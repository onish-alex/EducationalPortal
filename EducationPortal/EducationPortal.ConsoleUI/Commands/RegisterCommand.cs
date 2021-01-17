﻿using System;
using System.Collections.Generic;
using System.Text;
using EducationPortal.BLL.DTO;
using EducationPortal.BLL.Services;
using EducationPortal.ConsoleUI.Validation;
using EducationPortal.BLL.Response;

namespace EducationPortal.ConsoleUI.Commands
{
    public class RegisterCommand : ICommand<RegisterResponse>
    {
        public RegisterResponse Response { get; private set; }

        private IUserService reciever;
        private UserDTO user;
        private AccountDTO account;
        private RegisterDataValidator validator;

        public RegisterCommand(IUserService reciever, UserDTO user, AccountDTO account)
        {
            this.reciever = reciever;
            this.user = user;
            this.account = account;
            this.validator = new RegisterDataValidator(user, account);
        }

        public void Execute()
        {
            var validationResult = validator.Validate();
            Response = (validationResult.IsValid) ? reciever.Register(user, account)
                                                  : new RegisterResponse() { Message = validationResult.Message } ;
        }
    }
}
