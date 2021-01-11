using System;
using System.Collections.Generic;
using System.Text;
using EducationPortal.BLL.DTO;

namespace EducationPortal.BLL.Services
{
    public interface IUserService
    {
        string[] Register(UserDTO user);

        string[] Authorize(AccountDTO account);
    }
}
