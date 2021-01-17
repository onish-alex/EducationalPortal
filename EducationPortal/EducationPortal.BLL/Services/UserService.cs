using System;
using System.Linq;
using EducationPortal.DAL.Repository;
using EducationPortal.DAL.Entities;
using EducationPortal.BLL.DTO;
using AutoMapper;

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

            var userConfig = new MapperConfiguration(cfg => cfg.CreateMap<UserDTO, User>());
            this.userMapper = new Mapper(userConfig);

            var accountConfig = new MapperConfiguration(cfg => cfg.CreateMap<AccountDTO, Account>());
            this.accountMapper = new Mapper(accountConfig);
        }

        public string[] Authorize(AccountDTO account)
        {
            return null;
        }

        public string[] Register(UserDTO user, AccountDTO account)
        {
            var userToRegister = userMapper.Map<User>(user); 
            var accountToRegister = accountMapper.Map<Account>(account); 

            if (accounts.Find(account => account.Email.ToLower() == accountToRegister.Email).SingleOrDefault() != null)
            {
                return new string[] { "Данный Email уже занят!" };
            }

            if (accounts.Find(account => account.Login == accountToRegister.Login).SingleOrDefault() != null)
            {
                return new string[] { "Данный логин уже занят!" };
            }

            users.Create(userToRegister);
            accounts.Create(accountToRegister);

            users.Save();
            accounts.Save();

            return new string[] { "Новый пользователь зарегистрирован!" };
        }
    }
}
