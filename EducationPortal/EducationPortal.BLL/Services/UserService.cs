using System.Text;
using System.Security.Cryptography;
using System.Linq;
using EducationPortal.DAL.Repository;
using EducationPortal.DAL.Entities;
using EducationPortal.BLL.DTO;
using AutoMapper;
using EducationPortal.BLL.Response;

namespace EducationPortal.BLL.Services
{
    public class UserService : IUserService
    {
        private IRepository<User> users;
        private IRepository<Account> accounts;
        private IRepository<Skill> skills;
        private Mapper userMapper;
        private Mapper accountMapper;
        private Mapper skillMapper;

        public string Name => "User";

        public UserService(IRepository<User> users, IRepository<Account> accounts, IRepository<Skill> skills)
        {
            this.users = users;
            this.accounts = accounts;
            this.skills = skills;

            var skillConfig = new MapperConfiguration(cfg => cfg.CreateMap<SkillDTO, Skill>().ReverseMap());
            this.skillMapper = new Mapper(skillConfig);

            var userConfig = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<UserDTO, User>()
                .ForMember(dest => dest.Skills,
                           opt => opt.MapFrom(dto => dto.Skills.ToDictionary(key => skills.Find(skill => skill.Name == key.Key).Select(a => a.Id).SingleOrDefault(),
                                                                             value => value.Value.Skill)));

                cfg.CreateMap<User, UserDTO>()
                .ForMember(dest => dest.Skills,
                           opt => opt.MapFrom(entity => entity.Skills.ToDictionary(key => skills.GetById(key.Key).Name,
                                                                                   value => new UserSkillDTO() { Skill = skillMapper.Map<SkillDTO>(skills.GetById(value.Key)), Level = value.Value })));
            });
            this.userMapper = new Mapper(userConfig);

            var accountConfig = new MapperConfiguration(cfg => cfg.CreateMap<AccountDTO, Account>());
            this.accountMapper = new Mapper(accountConfig);
        }

        public AuthorizeResponse Authorize(AccountDTO account)
        {
            var responce = new AuthorizeResponse();

            var accountToLogIn = accountMapper.Map<Account>(account);

            var hash = GetPasswordHash(accountToLogIn.Password);

            var loggedInAccount = accounts.Find(account => (account.Email == accountToLogIn.Email.ToLower()
                                                         || account.Login == accountToLogIn.Login)
                                                         && account.Password == hash).SingleOrDefault();

            if (loggedInAccount == null)
            {
                responce.Message = "Неверно введенное имя пользователя, email или пароль!";
                return responce;
            }

            responce.Id = loggedInAccount.Id;
            responce.User = userMapper.Map<UserDTO>(users.GetById(loggedInAccount.Id));
            responce.IsSuccessful = true;
            
            return responce;
        }

        public OperationResponse Register(UserDTO user, AccountDTO account)
        {
            var responce = new OperationResponse();
            
            var userToRegister = userMapper.Map<User>(user); 
            var accountToRegister = accountMapper.Map<Account>(account); 

            if (accounts.Find(account => account.Email == accountToRegister.Email.ToLower()).SingleOrDefault() != null)
            {
                responce.Message = "Данный Email уже занят!";
                return responce;
            }

            if (accounts.Find(account => account.Login == accountToRegister.Login).SingleOrDefault() != null)
            {
                responce.Message = "Данный логин уже занят!";
                return responce;
            }

            accountToRegister.Email = accountToRegister.Email.ToLower();
            accountToRegister.Password = GetPasswordHash(accountToRegister.Password);

            users.Create(userToRegister);
            accounts.Create(accountToRegister);

            users.Save();
            accounts.Save();

            responce.Message = "Новый пользователь зарегистрирован!";
            return responce;
        } 

        public GetUserResponse GetUserById(long userId)
        {
            var response = new GetUserResponse();

            var user = users.GetById(userId);
            
            if (user == null)
            {
                response.IsSuccessful = false;
                response.Message = "Не удалось найти информацию о пользователе!";
                return response;
            }

            response.User = userMapper.Map<User, UserDTO>(user);
            response.Message = string.Empty;
            response.IsSuccessful = true;

            return response;
        }

        public OperationResponse JoinToCourse(long userId, long courseId)
        {
            var response = new GetUserResponse();

            var user = users.GetById(userId);

            if (user == null)
            {
                response.IsSuccessful = false;
                response.Message = "Указанного пользователя не существует!";
                return response;
            }

            if (user.JoinedCourseIds.Contains(courseId))
            {
                response.IsSuccessful = false;
                response.Message = "Вы уже проходите данный курс!";
                return response;
            }

            var userToUpdate = userMapper.Map<User>(user);
            userToUpdate.JoinedCourseIds = userToUpdate.JoinedCourseIds.Append(courseId).ToArray();
            users.Update(userToUpdate);
            users.Save();

            response.IsSuccessful = true;
            response.Message = "Начато изучение нового курса!";
            return response;

        }

        public OperationResponse AddLearnedMaterial(long userId, long materialId)
        {
            var response = new OperationResponse();

            var user = users.GetById(userId);

            if (user == null)
            {
                response.IsSuccessful = false;
                response.Message = "Указанного пользователя не существует!";
                return response;
            }

            if (user.CompletedMaterialIds.Contains(materialId))
            {
                response.IsSuccessful = false;
                response.Message = "Данный материал уже изучен!";
                return response;
            }

            user.CompletedMaterialIds = user.CompletedMaterialIds.Append(materialId).ToArray();

            users.Update(user);
            users.Save();

            response.IsSuccessful = true;
            response.Message = "Изучен новый материал!";
            return response;
        }

        public OperationResponse AddCompletedCourse(long userId, CourseDTO course)
        {
            var response = new OperationResponse();

            var user = users.GetById(userId);

            if (user == null)
            {
                response.IsSuccessful = false;
                response.Message = "Указанного пользователя не существует!";
                return response;
            }

            if (user.CompletedCourseIds.Contains(course.Id))
            {
                response.IsSuccessful = false;
                response.Message = "Данный курс уже изучен!";
                return response;
            }

            user.JoinedCourseIds = user.JoinedCourseIds.Except(Enumerable.Repeat(course.Id, 1)).ToArray();
            user.CompletedCourseIds = user.CompletedCourseIds.Append(course.Id).ToArray();
            
            foreach (var skillDTO in course.Skills)
            {
                var skill = skills.Find(a => a.Name == skillDTO.Name).SingleOrDefault();

                if (user.Skills.ContainsKey(skill.Id))
                {
                    user.Skills[skill.Id]++;
                }
                else
                {
                    user.Skills.Add(skill.Id, 1);
                }
            }

            users.Update(user);
            users.Save();

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
