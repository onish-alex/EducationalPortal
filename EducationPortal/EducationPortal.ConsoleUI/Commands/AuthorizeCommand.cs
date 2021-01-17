using System;
using System.Collections.Generic;
using System.Text;
using EducationPortal.BLL.DTO;
using EducationPortal.BLL.Services;

namespace EducationPortal.ConsoleUI.Commands
{
    public class AuthorizeCommand : ICommand
    {
        public string[] Result { get; private set; }

        private IUserService reciever;
        private AccountDTO account;

        public AuthorizeCommand(IUserService reciever, AccountDTO account)
        {
            this.reciever = reciever;
            this.account = account;
        }

        public void Execute()
        {
            Result = reciever.Authorize(account);
        }
    }
}
