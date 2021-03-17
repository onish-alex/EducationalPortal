namespace EducationPortal.ConsoleUI.Commands
{
    using System;
    using EducationPortal.BLL.DTO;
    using EducationPortal.BLL.Services;
    using EducationPortal.BLL.Validation;
    using EducationPortal.ConsoleUI.Resources;
    using EducationPortal.ConsoleUI.Utilities;

    public class AuthorizeCommand : ICommand
    {
        private IUserService userService;
        private ClientData client;
        private IValidator<AccountDTO> accountValidator;

        public AuthorizeCommand(
            IUserService userService,
            ClientData client,
            IValidator<AccountDTO> accountValidator)
        {
            this.userService = userService;
            this.client = client;
            this.accountValidator = accountValidator;
        }

        public string Name => "login";

        public string Description => "login [login | email] [password]\nАвторизация пользователя\n";

        public int ParamsCount => 2;

        public void Execute()
        {
            if (this.client.IsAuthorized)
            {
                Console.WriteLine(ConsoleMessages.ErrorTryLogInWhileLoggedIn);
                return;
            }

            var account = new AccountDTO()
            {
                Email = this.client.InputBuffer[0],
                Login = this.client.InputBuffer[0],
                Password = this.client.InputBuffer[1],
            };

            var validationResult = this.accountValidator.Validate(account, "Base");

            if (!validationResult.IsValid)
            {
                var errorCode = validationResult.Errors[0].ErrorCode;
                OutputHelper.PrintValidationError(errorCode);
                return;
            }

            var response = this.userService.Authorize(account);

            if (!response.IsSuccessful)
            {
                Console.WriteLine(ResourceHelper.GetMessageString(response.MessageCode));
                return;
            }

            this.client.IsAuthorized = true;
            this.client.Id = response.Id;
            this.client.Info = response.User;
            this.client.ConsoleStatePrefix = ConsoleMessages.UserPrefix + this.client.Info.Name;
        }
    }
}
