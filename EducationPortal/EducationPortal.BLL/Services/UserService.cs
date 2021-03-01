namespace EducationPortal.BLL.Services
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Security.Cryptography;
    using System.Text;
    using EducationPortal.BLL.DTO;
    using EducationPortal.BLL.Mappers;
    using EducationPortal.BLL.Response;
    using EducationPortal.DAL.Entities.EF;
    using EducationPortal.DAL.Repository.Base;

    public class UserService : IUserService
    {
        private IRepository<User> users;
        private IRepository<Account> accounts;
        private IRepository<Skill> skills;
        private IRepository<Course> courses;
        private IMapper mapper;

        public UserService(
            IRepository<User> users,
            IRepository<Account> accounts,
            IRepository<Skill> skills,
            IRepository<Course> courses,
            IMapper mapper)
        {
            this.users = users;
            this.accounts = accounts;
            this.skills = skills;
            this.courses = courses;
            this.mapper = mapper;
        }

        public string Name => "User";

        public AuthorizeResult Authorize(AccountDTO account)
        {
            var responce = new AuthorizeResult();

            var accountToLogIn = this.mapper.Map<AccountDTO, Account>(account);

            var hash = this.GetPasswordHash(accountToLogIn.Password);

            var loggedInAccount = this.accounts.Find(
                account => (account.Email == accountToLogIn.Email.ToLower()
                         || account.Login == accountToLogIn.Login)
                         && account.Password == hash,
                account => account.User)
                .SingleOrDefault();

            if (loggedInAccount == null)
            {
                responce.IsSuccessful = false;
                responce.MessageCode = "AuthorizeWrongCredentials";
                return responce;
            }

            responce.Id = loggedInAccount.Id;
            responce.User = this.mapper.Map<User, UserDTO>(loggedInAccount.User);
            responce.IsSuccessful = true;

            return responce;
        }

        public OperationResult Register(UserDTO user, AccountDTO account)
        {
            var responce = new OperationResult();

            var userToRegister = this.mapper.Map<UserDTO, User>(user);
            var accountToRegister = this.mapper.Map<AccountDTO, Account>(account);

            if (this.accounts.Find(account => account.Email == accountToRegister.Email.ToLower()).SingleOrDefault() != null)
            {
                responce.MessageCode = "RegisterEmailUsed";
                return responce;
            }

            if (this.accounts.Find(account => account.Login == accountToRegister.Login).SingleOrDefault() != null)
            {
                responce.MessageCode = "RegisterLoginUsed";
                return responce;
            }

            accountToRegister.Email = accountToRegister.Email.ToLower();
            accountToRegister.Password = this.GetPasswordHash(accountToRegister.Password);
            accountToRegister.User = userToRegister;

            this.accounts.Create(accountToRegister);
            this.accounts.Save();

            responce.IsSuccessful = true;
            responce.MessageCode = "RegisterSuccess";
            return responce;
        }

        public GetUserInfoResult GetUserById(long userId)
        {
            var response = new GetUserInfoResult();

            var user = this.users.Find(
                user => user.Id == userId,
                user => user.JoinedCourses,
                user => user.CompletedCourses,
                user => user.LearnedMaterials,
                user => user.UserSkills)
                .SingleOrDefault();

            if (user == null)
            {
                response.IsSuccessful = false;
                response.MessageCode = "GetUserByIdNotFound";
                return response;
            }

            response.User = this.mapper.Map<User, UserDTO>(user);
            response.CompletedCourses = this.mapper.Map<Course, CourseDTO>(user.CompletedCourses.Select(x => x.Course));

            var joinedCourseProgress = new Dictionary<CourseDTO, int>();
            var joinedCourses = this.courses.Find(
                x => user.JoinedCourses.Select(y => y.CourseId).Contains(x.Id),
                x => x.Materials);

            foreach (var course in joinedCourses)
            {
                var completedMaterialCount = course.Materials.Intersect(user.LearnedMaterials).Count();
                var allMaterialCount = course.Materials.Count();

                var percent = (allMaterialCount != 0)
                    ? Math.Round(completedMaterialCount / (double)allMaterialCount, 2)
                    : 0;

                joinedCourseProgress.Add(this.mapper.Map<Course, CourseDTO>(course), (int)(percent * 100));
            }

            response.JoinedCoursesProgress = joinedCourseProgress;

            var userSkills = this.skills.Find(x => user.UserSkills.Select(a => a.SkillId).Contains(x.Id));

            response.SkillLevels = userSkills.ToDictionary(k => this.mapper.Map<Skill, SkillDTO>(k), v => user.UserSkills.First(x => x.SkillId == v.Id).Level);
            response.IsSuccessful = true;

            return response;
        }

        public OperationResult JoinToCourse(long userId, long courseId)
        {
            var response = new GetUserInfoResult();

            var user = this.users.Find(
                user => user.Id == userId,
                user => user.JoinedCourses)
                .SingleOrDefault();

            if (user == null)
            {
                response.IsSuccessful = false;
                response.MessageCode = "UserNotFound";
                return response;
            }

            var course = this.courses.Find(
                course => course.Id == courseId,
                course => course.JoinedUsers,
                course => course.CompletedUsers)
                .SingleOrDefault();

            if (course == null)
            {
                response.IsSuccessful = false;
                response.MessageCode = "CourseNotFound";
                return response;
            }

            if (course.JoinedUsers.Any(x => x.UserId == userId))
            {
                response.IsSuccessful = false;
                response.MessageCode = "CourseAlreadyJoin";
                return response;
            }

            if (course.CompletedUsers.Any(x => x.UserId == userId))
            {
                response.IsSuccessful = false;
                response.MessageCode = "CourseAlreadyCompleted";
                return response;
            }

            user.JoinedCourses.Add(new UserJoinedCourses()
            {
                UserId = (int)userId,
                CourseId = (int)courseId,
            });

            this.users.Update(user);
            this.users.Save();

            response.IsSuccessful = true;
            response.MessageCode = "JoinToCourseSuccess";
            return response;
        }

        public CompletedCourseResult AddCompletedCourse(long userId, long courseId)
        {
            var response = new CompletedCourseResult();

            var user = this.users.Find(
                user => user.Id == userId,
                user => user.CompletedCourses,
                user => user.JoinedCourses,
                user => user.LearnedMaterials,
                user => user.UserSkills)
                .SingleOrDefault();

            if (user == null)
            {
                response.IsSuccessful = false;
                response.MessageCode = "UserNotFound";
                return response;
            }

            var course = this.courses.Find(
                course => course.Id == courseId,
                course => course.Materials,
                course => course.Skills)
                .SingleOrDefault();

            if (course == null)
            {
                response.IsSuccessful = false;
                response.MessageCode = "CourseNotFound";
                return response;
            }

            if (user.CompletedCourses.Select(x => x.Course).Contains(course))
            {
                response.IsSuccessful = false;
                response.MessageCode = "CourseAlreadyCompleted";
                return response;
            }

            if (!user.JoinedCourses.Select(x => x.Course).Contains(course))
            {
                response.IsSuccessful = false;
                response.MessageCode = "CourseNotJoined";
                return response;
            }

            if (course.Materials.Any(x => !user.LearnedMaterials.Contains(x)))
            {
                response.IsSuccessful = false;
                response.MessageCode = "AddCompletedCourseNotCompleted";
                return response;
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

            response.RecievedSkills = user.UserSkills
                                          .Where(x => course.Skills.Contains(x.Skill))
                                          .ToDictionary(k => this.mapper.Map<Skill, SkillDTO>(k.Skill), v => v.Level);

            this.users.Update(user);
            this.users.Save();

            response.IsSuccessful = true;
            return response;
        }

        public GetCoursesResult GetJoinedCourses(long userId)
        {
            var response = new GetCoursesResult();

            var user = this.users.Find(
                user => user.Id == userId,
                user => user.JoinedCourses)
                .SingleOrDefault();

            if (user == null)
            {
                response.IsSuccessful = false;
                response.MessageCode = "UserNotFound";
                return response;
            }

            var joinedCourseIds = user.JoinedCourses.Select(x => x.CourseId);
            var joinedCourses = this.courses.Find(
                course => joinedCourseIds.Contains(course.Id),
                course => course.Skills);

            response.Courses = this.mapper.Map<Course, CourseDTO>(joinedCourses);

            response.IsSuccessful = true;
            return response;
        }

        public GetCoursesResult GetCompletedCourses(long userId)
        {
            var response = new GetCoursesResult();

            var user = this.users.Find(
                            user => user.Id == userId,
                            user => user.CompletedCourses)
                            .SingleOrDefault();

            if (user == null)
            {
                response.IsSuccessful = false;
                response.MessageCode = "UserNotFound";
                return response;
            }

            var completedCourseIds = user.CompletedCourses.Select(x => x.CourseId);
            var completedCourses = this.courses.Find(
                course => completedCourseIds.Contains(course.Id),
                course => course.Skills);

            response.Courses = this.mapper.Map<Course, CourseDTO>(completedCourses);

            response.IsSuccessful = true;
            return response;
        }

        public GetMaterialsResult GetNextMaterial(long userId, long courseId)
        {
            var response = new GetMaterialsResult();

            var user = this.users.Find(
                user => user.Id == userId,
                user => user.LearnedMaterials,
                user => user.JoinedCourses)
                .SingleOrDefault();

            if (user == null)
            {
                response.IsSuccessful = false;
                response.MessageCode = "UserNotFound";
                return response;
            }

            var course = this.courses.Find(
                course => course.Id == courseId,
                course => course.Materials)
                .SingleOrDefault();

            if (!user.JoinedCourses.Select(x => x.Course).Contains(course))
            {
                response.IsSuccessful = false;
                response.MessageCode = "CourseNotJoined";
                return response;
            }

            var materialToLearn = course.Materials.FirstOrDefault(x => !user.LearnedMaterials.Contains(x));

            if (materialToLearn == null)
            {
                response.IsSuccessful = false;
                response.MessageCode = "GetNextMaterialAnyNewMaterial";
                return response;
            }

            user.LearnedMaterials.Add(materialToLearn);

            this.users.Update(user);
            this.users.Save();

            response.Materials = new MaterialDTO[] { this.mapper.Map<Material, MaterialDTO>(materialToLearn) };
            response.IsSuccessful = true;
            return response;
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
