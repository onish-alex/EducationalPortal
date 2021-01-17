using System;
using System.Collections.Generic;
using System.Text;
using EducationPortal.BLL.DTO;

namespace EducationPortal.BLL.Services
{
    public interface IUserService : IService
    {
        string[] Register(UserDTO user, AccountDTO account);

        string[] Authorize(AccountDTO account);
    }
}
