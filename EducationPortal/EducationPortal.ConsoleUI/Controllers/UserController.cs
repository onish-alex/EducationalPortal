using System;
using System.Collections.Generic;
using System.Text;
using EducationPortal.BLL.Services;

namespace EducationPortal.ConsoleUI.Controllers
{
    public class UserController
    {
        private IUserService userService;

        public UserController(IUserService userService)
        {
            this.userService = userService;
        }
    }
}
