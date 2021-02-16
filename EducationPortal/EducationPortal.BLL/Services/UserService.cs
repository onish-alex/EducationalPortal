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
        private CommonMapper mapper;

        public UserService(
            IRepository<User> users,
            IRepository<Account> accounts,
            IRepository<Skill> skills,
            IRepository<Course> courses)
        {
            this.users = users;
            this.accounts = accounts;
            this.skills = skills;
            this.courses = courses;
            this.mapper = CommonMapper.GetInstance();
        }

        public string Name => "User";

        public AuthorizeResponse Authorize(AccountDTO account)
        {
            var responce = new AuthorizeResponse();

            var accountToLogIn = this.mapper.Map<AccountDTO, Account>(account);

            var hash = this.GetPasswordHash(accountToLogIn.Password);

            var loggedInAccount = this.accounts.Find(account => (account.Email == accountToLogIn.Email.ToLower()
                                                         || account.Login == accountToLogIn.Login)
                                                         && account.Password == hash).SingleOrDefault();

            if (loggedInAccount == null)
            {
                responce.Message = "Неверно введенное имя пользователя, email или пароль!";
                return responce;
            }

            responce.Id = loggedInAccount.Id;
            responce.User = this.mapper.Map<User, UserDTO>(this.users.GetById(loggedInAccount.Id));
            responce.IsSuccessful = true;

            return responce;
        }

        public OperationResponse Register(UserDTO user, AccountDTO account)
        {
            var responce = new OperationResponse();

            var userToRegister = this.mapper.Map<UserDTO, User>(user);
            var accountToRegister = this.mapper.Map<AccountDTO, Account>(account);

            if (this.accounts.Find(account => account.Email == accountToRegister.Email.ToLower()).SingleOrDefault() != null)
            {
                responce.Message = "Данный Email уже занят!";
                return responce;
            }

            if (this.accounts.Find(account => account.Login == accountToRegister.Login).SingleOrDefault() != null)
            {
                responce.Message = "Данный логин уже занят!";
                return responce;
            }

            accountToRegister.Email = accountToRegister.Email.ToLower();
            accountToRegister.Password = this.GetPasswordHash(accountToRegister.Password);
            accountToRegister.User = userToRegister;

            this.accounts.Create(accountToRegister);
            this.accounts.Save();

            responce.Message = "Новый пользователь зарегистрирован!";
            return responce;
        }

        public GetUserInfoResponse GetUserById(long userId)
        {
            var response = new GetUserInfoResponse();

            var user = this.users.GetById(userId);

            if (user == null)
            {
                response.IsSuccessful = false;
                response.Message = "Не удалось найти информацию о пользователе!";
                return response;
            }

            response.User = this.mapper.Map<User, UserDTO>(user);

            var completedCourses = this.courses.Find(x => user.CompletedCourses.Select(a => a.CourseId).Contains(x.Id));
            response.CompletedCourses = this.mapper.Map<Course, CourseDTO>(completedCourses);

            var joinedCourseProgress = new Dictionary<CourseDTO, int>();
            var joinedCourses = this.courses.Find(x => user.JoinedCourses.Select(y => y.CourseId).Contains(x.Id));

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
            response.Message = string.Empty;
            response.IsSuccessful = true;

            return response;
        }

        public OperationResponse JoinToCourse(long userId, long courseId)
        {
            var response = new GetUserInfoResponse();

            var user = this.users.GetById(userId);

            if (user == null)
            {
                response.IsSuccessful = false;
                response.Message = "Указанного пользователя не существует!";
                return response;
            }

            if (user.JoinedCourses.Any(x => x.CourseId == courseId))
            {
                response.IsSuccessful = false;
                response.Message = "Вы уже проходите данный курс!";
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
            response.Message = "Начато изучение нового курса!";
            return response;
        }

        public CompletedCourseResponse AddCompletedCourse(long userId, long courseId)
        {
            var response = new CompletedCourseResponse();

            var user = this.users.GetById(userId);

            if (user == null)
            {
                response.IsSuccessful = false;
                response.Message = "Указанного пользователя не существует!";
                return response;
            }

            var course = this.courses.GetById(courseId);

            if (user.CompletedCourses.Select(x => x.Course).Contains(course))
            {
                response.IsSuccessful = false;
                response.Message = "Данный курс уже изучен!";
                return response;
            }

            if (course.Materials.Any(x => !user.LearnedMaterials.Contains(x)))
            {
                response.IsSuccessful = false;
                response.Message = "Изучены не все материалы курса!";
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
                    });
                }
                else
                {
                    user.UserSkills.Remove(userSkill);
                    userSkill.Level++;
                    user.UserSkills.Add(userSkill);
                }
            }

            var recievedSkills = new Dictionary<SkillDTO, int>();

            response.RecievedSkills = user.UserSkills
                                          .Where(x => course.Skills.Contains(x.Skill))
                                          .ToDictionary(k => this.mapper.Map<Skill, SkillDTO>(k.Skill), v => v.Level);

            this.users.Update(user);
            this.users.Save();

            response.IsSuccessful = true;
            return response;
        }

        public GetCoursesResponse GetJoinedCourses(long userId)
        {
            var response = new GetCoursesResponse();

            var user = this.users.GetById(userId);

            if (user == null)
            {
                response.IsSuccessful = false;
                response.Message = "Указанного пользователя не существует!";
                return response;
            }

            response.Courses = this.mapper.Map<Course, CourseDTO>(user.JoinedCourses.Select(x => x.Course));
            response.IsSuccessful = true;
            return response;
        }

        public GetCoursesResponse GetCompletedCourses(long userId)
        {
            var response = new GetCoursesResponse();

            var user = this.users.GetById(userId);

            if (user == null)
            {
                response.IsSuccessful = false;
                response.Message = "Указанного пользователя не существует!";
                return response;
            }

            response.Courses = this.mapper.Map<Course, CourseDTO>(user.CompletedCourses.Select(x => x.Course));
            response.IsSuccessful = true;
            return response;
        }

        public GetMaterialsResponse GetNextMaterial(int userId, int courseId)
        {
            var response = new GetMaterialsResponse();

            var user = this.users.GetById(userId);

            if (user == null)
            {
                response.IsSuccessful = false;
                response.Message = "Указанного пользователя не существует!";
                return response;
            }

            var course = this.courses.GetById(courseId);

            var materialToLearn = course.Materials.FirstOrDefault(x => !user.LearnedMaterials.Contains(x));

            if (materialToLearn == null)
            {
                response.IsSuccessful = false;
                response.Message = "Все материалы курса пройдены!";
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
