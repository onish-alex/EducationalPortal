using System;
using System.Collections.Generic;
using System.Text;
using EducationPortal.BLL.Response;
using EducationPortal.BLL.DTO;
using EducationPortal.BLL.Services;

namespace EducationPortal.ConsoleUI.Commands
{
    public class GetUserCommand : ICommand<GetUserResponse>
    {
        private IUserService userService;

        public GetUserResponse Response { get; set; }

        private long userId;

        public GetUserCommand(IUserService userService, long userId)
        {
            this.userService = userService;
            this.userId = userId;
        }

        public void Execute()
        {
            Response = userService.GetUserById(userId);
        }
    }
}
