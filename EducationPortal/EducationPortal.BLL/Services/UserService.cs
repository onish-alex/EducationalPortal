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
        private Mapper userMapper;
        private Mapper accountMapper;

        public string Name => "User";

        public UserService(IRepository<User> users, IRepository<Account> accounts)
        {
            this.users = users;
            this.accounts = accounts;

            var userConfig = new MapperConfiguration(cfg => cfg.CreateMap<UserDTO, User>().ReverseMap());
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

        public RegisterResponse Register(UserDTO user, AccountDTO account)
        {
            var responce = new RegisterResponse();
            
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
