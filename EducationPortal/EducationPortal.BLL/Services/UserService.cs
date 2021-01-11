using System;
using EducationPortal.DAL.Repository;
using EducationPortal.BLL.DTO;

namespace EducationPortal.BLL.Services
{
    public class UserService : IUserService
    {
        private IUserRepository users;
        private IAccountRepository accounts;

        public UserService(IUserRepository users, IAccountRepository accounts)
        {
            this.users = users;
            this.accounts = accounts;
        }

        public string[] Authorize(AccountDTO account)
        {
            throw new NotImplementedException();
        }

        public string[] Register(UserDTO user)
        {
            throw new NotImplementedException();
        }
    }
}
