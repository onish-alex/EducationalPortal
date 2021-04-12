namespace EducationPortal.BLL.Services
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Security.Cryptography;
    using System.Text;
    using System.Threading.Tasks;
    using EducationPortal.BLL.DTO;
    using EducationPortal.BLL.Mappers;
    using EducationPortal.BLL.Results;
    using EducationPortal.BLL.Utilities;
    using EducationPortal.DAL.Entities.EF;
    using EducationPortal.DAL.Repository.Base;
    using FluentValidation;

    public class UserService : IUserService
    {
        private IRepository<User> userRepository;
        private IRepository<Account> accountRepository;
        private IRepository<Skill> skillRepository;
        private IRepository<Course> courseRepository;
        private IMapper mapper;
        private IValidator<AccountDTO> accountValidator;
        private IValidator<UserDTO> userValidator;

        public UserService(
            IRepository<User> userRepository,
            IRepository<Account> accountRepository,
            IRepository<Skill> skillRepository,
            IRepository<Course> courseRepository,
            IMapper mapper,
            IValidator<AccountDTO> accountValidator,
            IValidator<UserDTO> userValidator)
        {
            this.userRepository = userRepository;
            this.accountRepository = accountRepository;
            this.skillRepository = skillRepository;
            this.courseRepository = courseRepository;
            this.mapper = mapper;
            this.accountValidator = accountValidator;
            this.userValidator = userValidator;
        }

        public AuthorizeResult Authorize(AccountDTO account)
        {
            var result = new AuthorizeResult();
            if (account == null)
            {
                result.IsSuccessful = false;
                result.MessageCode = "AccountNull";
                return result;
            }

            var validationResult = this.accountValidator.Validate(account, "Base");

            if (!validationResult.IsValid)
            {
                result.IsSuccessful = false;
                result.MessageCode = validationResult.Errors[0].ErrorCode;
                return result;
            }

            var accountToLogIn = this.mapper.Map<AccountDTO, Account>(account);

            var hash = this.GetPasswordHash(accountToLogIn.Password);

            var loggedInAccount = this.accountRepository.Find(
                account => (account.Email == accountToLogIn.Email.ToLower()
                         || account.Login == accountToLogIn.Login)
                         && account.Password == hash,
                account => account.User)
                .SingleOrDefault();

            if (loggedInAccount == null)
            {
                result.IsSuccessful = false;
                result.MessageCode = "AuthorizeWrongCredentials";
                return result;
            }

            result.Id = loggedInAccount.Id;
            result.User = this.mapper.Map<User, UserDTO>(loggedInAccount.User);
            result.IsSuccessful = true;

            return result;
        }

        public async Task<OperationResult> Register(UserDTO user, AccountDTO account)
        {
            var result = new OperationResult();

            if (account == null)
            {
                result.IsSuccessful = false;
                result.MessageCode = "AccountNull";
                return result;
            }

            if (user == null)
            {
                result.IsSuccessful = false;
                result.MessageCode = "UserNull";
                return result;
            }

            var validationResult = this.accountValidator.Validate(account, "Base", "Detail");

            if (!validationResult.IsValid)
            {
                result.IsSuccessful = false;
                result.MessageCode = validationResult.Errors[0].ErrorCode;
                return result;
            }

            validationResult = this.userValidator.Validate(user);

            if (!validationResult.IsValid)
            {
                result.IsSuccessful = false;
                result.MessageCode = validationResult.Errors[0].ErrorCode;
                return result;
            }

            var userToRegister = this.mapper.Map<UserDTO, User>(user);
            var accountToRegister = this.mapper.Map<AccountDTO, Account>(account);

            if (this.accountRepository.Find(account => account.Email == accountToRegister.Email.ToLower()).SingleOrDefault() != null)
            {
                result.MessageCode = "RegisterEmailUsed";
                return result;
            }

            if (this.accountRepository.Find(account => account.Login == accountToRegister.Login).SingleOrDefault() != null)
            {
                result.MessageCode = "RegisterLoginUsed";
                return result;
            }

            accountToRegister.Email = accountToRegister.Email.ToLower();
            accountToRegister.Password = this.GetPasswordHash(accountToRegister.Password);
            accountToRegister.User = userToRegister;

            await this.accountRepository.CreateAsync(accountToRegister);
            await this.accountRepository.SaveAsync();

            result.IsSuccessful = true;
            result.MessageCode = "RegisterSuccess";
            return result;
        }

        public GetUserInfoResult GetUserById(long userId)
        {
            var result = new GetUserInfoResult();

            var user = this.userRepository.Find(
                user => user.Id == userId,
                user => user.JoinedCourses,
                user => user.CompletedCourses,
                user => user.LearnedMaterials,
                user => user.UserSkills)
                .SingleOrDefault();

            if (user == null)
            {
                result.IsSuccessful = false;
                result.MessageCode = "GetUserByIdNotFound";
                return result;
            }

            result.User = this.mapper.Map<User, UserDTO>(user);
            result.CompletedCourses = this.mapper.Map<Course, CourseDTO>(user.CompletedCourses.Select(x => x.Course));

            var joinedCourseProgress = new Dictionary<CourseDTO, int>();
            var joinedCourses = this.courseRepository.Find(
                x => user.JoinedCourses.Select(y => y.CourseId).Contains(x.Id),
                x => x.Materials);

            foreach (var course in joinedCourses)
            {
                var completedMaterialCount = course.Materials.Intersect(user.LearnedMaterials).Count();
                var allMaterialCount = course.Materials.Count;

                var percent = (allMaterialCount != 0)
                    ? Math.Round(completedMaterialCount / (double)allMaterialCount, 2)
                    : 0;

                joinedCourseProgress.Add(this.mapper.Map<Course, CourseDTO>(course), (int)(percent * 100));
            }

            result.JoinedCoursesProgress = joinedCourseProgress;

            var userSkills = this.skillRepository.Find(x => user.UserSkills.Select(a => a.SkillId).Contains(x.Id));

            result.SkillLevels = userSkills.ToDictionary(k => this.mapper.Map<Skill, SkillDTO>(k), v => user.UserSkills.First(x => x.SkillId == v.Id).Level);
            result.IsSuccessful = true;

            return result;
        }

        public async Task<OperationResult> JoinToCourse(long userId, long courseId)
        {
            var result = new GetUserInfoResult();

            var user = this.userRepository.Find(
                user => user.Id == userId,
                user => user.JoinedCourses)
                .SingleOrDefault();

            if (user == null)
            {
                result.IsSuccessful = false;
                result.MessageCode = "UserNotFound";
                return result;
            }

            var course = this.courseRepository.Find(
                course => course.Id == courseId,
                course => course.JoinedUsers,
                course => course.CompletedUsers)
                .SingleOrDefault();

            if (course == null)
            {
                result.IsSuccessful = false;
                result.MessageCode = "CourseNotFound";
                return result;
            }

            if (course.JoinedUsers.Any(x => x.UserId == userId))
            {
                result.IsSuccessful = false;
                result.MessageCode = "CourseAlreadyJoin";
                return result;
            }

            if (course.CompletedUsers.Any(x => x.UserId == userId))
            {
                result.IsSuccessful = false;
                result.MessageCode = "CourseAlreadyCompleted";
                return result;
            }

            user.JoinedCourses.Add(new UserJoinedCourses()
            {
                UserId = (int)userId,
                CourseId = (int)courseId,
            });

            this.userRepository.Update(user);
            await this.userRepository.SaveAsync();

            result.IsSuccessful = true;
            result.MessageCode = "JoinToCourseSuccess";
            return result;
        }

        public async Task<CompletedCourseResult> AddCompletedCourse(long userId, long courseId)
        {
            var result = new CompletedCourseResult();

            var user = this.userRepository.Find(
                user => user.Id == userId,
                user => user.CompletedCourses,
                user => user.JoinedCourses,
                user => user.LearnedMaterials,
                user => user.UserSkills)
                .SingleOrDefault();

            if (user == null)
            {
                result.IsSuccessful = false;
                result.MessageCode = "UserNotFound";
                return result;
            }

            var course = this.courseRepository.Find(
                course => course.Id == courseId,
                course => course.Materials,
                course => course.Skills)
                .SingleOrDefault();

            if (course == null)
            {
                result.IsSuccessful = false;
                result.MessageCode = "CourseNotFound";
                return result;
            }

            if (user.CompletedCourses.Select(x => x.Course).Contains(course))
            {
                result.IsSuccessful = false;
                result.MessageCode = "CourseAlreadyCompleted";
                return result;
            }

            if (!user.JoinedCourses.Select(x => x.Course).Contains(course))
            {
                result.IsSuccessful = false;
                result.MessageCode = "CourseNotJoined";
                return result;
            }

            if (course.Materials.Any(x => !user.LearnedMaterials.Contains(x)))
            {
                result.IsSuccessful = false;
                result.MessageCode = "AddCompletedCourseNotCompleted";
                return result;
            }

            user.JoinedCourses.Remove(user.JoinedCourses.Single(x => x.CourseId == courseId));
            user.CompletedCourses.Add(new UserCompletedCourses()
            {
                CourseId = (int)courseId,
                UserId = (int)userId,
            });

            foreach (var skill in course.Skills)
            {
                var userSkill = user.UserSkills.SingleOrDefault(x => x.SkillId == skill.Id);

                if (userSkill == null)
                {
                    user.UserSkills.Add(new UserSkills()
                    {
                        SkillId = skill.Id,
                        UserId = (int)userId,
                        Level = 1,
                        Skill = skill,
                    });
                }
                else
                {
                    user.UserSkills.Remove(userSkill);
                    userSkill.Level++;
                    user.UserSkills.Add(userSkill);
                }
            }

            var smth = user.UserSkills.Where(x => course.Skills.Contains(x.Skill));

            result.RecievedSkills = user.UserSkills
                                          .Where(x => course.Skills.Contains(x.Skill))
                                          .ToDictionary(k => this.mapper.Map<Skill, SkillDTO>(k.Skill), v => v.Level);

            this.userRepository.Update(user);
            await this.userRepository.SaveAsync();

            result.IsSuccessful = true;
            return result;
        }

        public GetCoursesResult GetJoinedCourses(long userId)
        {
            var result = new GetCoursesResult();

            var user = this.userRepository.Find(
                user => user.Id == userId,
                user => user.JoinedCourses)
                .SingleOrDefault();

            if (user == null)
            {
                result.IsSuccessful = false;
                result.MessageCode = "UserNotFound";
                return result;
            }

            var joinedCourseIds = user.JoinedCourses.Select(x => x.CourseId);
            var joinedCourses = this.courseRepository.Find(
                course => joinedCourseIds.Contains(course.Id),
                course => course.Skills);

            result.Courses = this.mapper.Map<Course, CourseDTO>(joinedCourses);

            result.IsSuccessful = true;
            return result;
        }

        public GetCoursesResult GetCompletedCourses(long userId)
        {
            var result = new GetCoursesResult();

            var user = this.userRepository.Find(
                            user => user.Id == userId,
                            user => user.CompletedCourses)
                            .SingleOrDefault();

            if (user == null)
            {
                result.IsSuccessful = false;
                result.MessageCode = "UserNotFound";
                return result;
            }

            var completedCourseIds = user.CompletedCourses.Select(x => x.CourseId);
            var completedCourses = this.courseRepository.Find(
                course => completedCourseIds.Contains(course.Id),
                course => course.Skills);

            result.Courses = this.mapper.Map<Course, CourseDTO>(completedCourses);

            result.IsSuccessful = true;
            return result;
        }

        public async Task<GetMaterialsResult> GetNextMaterial(long userId, long courseId)
        {
            var result = new GetMaterialsResult();

            var user = this.userRepository.Find(
                user => user.Id == userId,
                user => user.LearnedMaterials,
                user => user.JoinedCourses)
                .SingleOrDefault();

            if (user == null)
            {
                result.IsSuccessful = false;
                result.MessageCode = "UserNotFound";
                return result;
            }

            var course = this.courseRepository.Find(
                course => course.Id == courseId,
                course => course.Materials)
                .SingleOrDefault();

            if (!user.JoinedCourses.Select(x => x.Course).Contains(course))
            {
                result.IsSuccessful = false;
                result.MessageCode = "CourseNotJoined";
                return result;
            }

            var materialToLearn = course.Materials.FirstOrDefault(x => !user.LearnedMaterials.Contains(x));

            if (materialToLearn == null)
            {
                result.IsSuccessful = false;
                result.MessageCode = "GetNextMaterialAnyNewMaterial";
                return result;
            }

            user.LearnedMaterials.Add(materialToLearn);

            this.userRepository.Update(user);
            await this.userRepository.SaveAsync();

            result.Materials = new MaterialDTO[] { this.mapper.Map<Material, MaterialDTO>(materialToLearn) };
            result.IsSuccessful = true;
            return result;
        }

        public async Task<OperationResult> LearnMaterial(long userId, long courseId, long materialId)
        {
            var result = new OperationResult();

            var user = this.userRepository.Find(
                user => user.Id == userId,
                user => user.LearnedMaterials,
                user => user.JoinedCourses)
                .SingleOrDefault();

            if (user == null)
            {
                result.IsSuccessful = false;
                result.MessageCode = "UserNotFound";
                return result;
            }

            var course = this.courseRepository.Find(
                course => course.Id == courseId,
                course => course.Materials)
                .SingleOrDefault();

            if (course == null)
            {
                result.IsSuccessful = false;
                result.MessageCode = "CourseNotFound";
                return result;
            }

            if (!user.JoinedCourses.Any(x => x.Course == course))
            {
                result.IsSuccessful = false;
                result.MessageCode = "CourseNotJoined";
                return result;
            }

            var materialToLearn = course.Materials.SingleOrDefault(x => x.Id == materialId);

            if (materialToLearn == null)
            {
                result.IsSuccessful = false;
                result.MessageCode = "MaterialNotFound";
                return result;
            }

            user.LearnedMaterials.Add(materialToLearn);

            this.userRepository.Update(user);
            await this.userRepository.SaveAsync();

            result.IsSuccessful = true;
            return result;
        }

        private string GetPasswordHash(string password)
        {
            var hash = SHA512.Create().ComputeHash(Encoding.ASCII.GetBytes(password));

            var builder = new StringBuilder();
            for (int i = 0; i < hash.Length; i++)
            {
                builder.Append(hash[i].ToString("x2"));
            }

            return builder.ToString();
        }
    }
}
