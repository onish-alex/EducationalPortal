using System;
using System.Collections.Generic;
using System.Text;
using EducationPortal.BLL.DTO;
using EducationPortal.BLL.Services;

namespace EducationPortal.ConsoleUI.Commands
{
    public class RegisterCommand : IConsoleCommand
    {
        public string[] Result { get; private set; }

        private IUserService reciever;
        private UserDTO user;

        public RegisterCommand(IUserService reciever, UserDTO user)
        {
            this.reciever = reciever;
            this.user = user;
        }

        public void Execute()
        {
            Result = reciever.Register(user);
        }
    }
}
