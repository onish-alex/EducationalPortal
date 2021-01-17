using System;
using System.Collections.Generic;
using System.Text;
using EducationPortal.BLL.DTO;
using EducationPortal.BLL.Services;
using EducationPortal.BLL.Response;

namespace EducationPortal.ConsoleUI.Commands
{
    public class AuthorizeCommand : ICommand<AuthorizeResponse>
    {
        public AuthorizeResponse Response { get; private set; }

        private IUserService reciever;
        private AccountDTO account;

        public AuthorizeCommand(IUserService reciever, AccountDTO account)
        {
            this.reciever = reciever;
            this.account = account;
        }

        public void Execute()
        {
            Response = reciever.Authorize(account);
        }
    }
}
