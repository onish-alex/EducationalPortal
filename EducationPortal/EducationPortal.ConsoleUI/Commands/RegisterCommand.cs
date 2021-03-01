namespace EducationPortal.ConsoleUI.Commands
{
    using System;
    using EducationPortal.BLL.DTO;
    using EducationPortal.BLL.Services;
    using EducationPortal.BLL.Validation;
    using EducationPortal.ConsoleUI;
    using EducationPortal.ConsoleUI.Resources;
    using EducationPortal.ConsoleUI.Utilities;

    public class RegisterCommand : ICommand
    {
        private IUserService userService;
        private ClientData client;
        private UserValidator userValidator;
        private AccountValidator accountValidator;

        public RegisterCommand(
            IUserService userService,
            UserValidator userValidator,
            AccountValidator accountValidator,
            ClientData client)
        {
            this.userService = userService;
            this.client = client;
            this.userValidator = userValidator;
            this.accountValidator = accountValidator;
        }

        public string Name => "reg";

        public string Description => "reg [email] [login] [password] [username]\nРегистрация нового пользователя\n";

        public int ParamsCount => 4;

        public void Execute()
        {
            if (this.client.IsAuthorized)
            {
                Console.WriteLine(ConsoleMessages.ErrorTryRegWhileLoggedIn);
                return;
            }

            var account = new AccountDTO()
            {
                Email = this.client.InputBuffer[0],
                Login = this.client.InputBuffer[1],
                Password = this.client.InputBuffer[2],
            };

            var user = new UserDTO()
            {
                Name = this.client.InputBuffer[3],
            };

            var validationResult = this.accountValidator.Validate(account);

            if (!validationResult.IsValid)
            {
                var errorCode = validationResult.Errors[0].ErrorCode;
                OutputHelper.PrintValidationError(errorCode);
                return;
            }

            validationResult = this.userValidator.Validate(user);

            if (!validationResult.IsValid)
            {
                var errorCode = validationResult.Errors[0].ErrorCode;
                OutputHelper.PrintValidationError(errorCode);
                return;
            }

            var response = this.userService.Register(user, account);

            Console.WriteLine(OperationMessages.GetString(response.MessageCode));
        }
    }
}
