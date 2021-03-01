namespace EducationPortal.ConsoleUI.Commands
{
    using System;
    using EducationPortal.BLL.DTO;
    using EducationPortal.BLL.Services;
    using EducationPortal.ConsoleUI.Resources;

    public class AuthorizeCommand : ICommand
    {
        private IUserService userService;
        private ClientData client;

        public AuthorizeCommand(IUserService userService, ClientData client)
        {
            this.userService = userService;
            this.client = client;
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

            var response = this.userService.Authorize(account);

            if (!response.IsSuccessful)
            {
                Console.WriteLine(OperationMessages.GetString(response.MessageCode));
                return;
            }

            this.client.IsAuthorized = true;
            this.client.Id = response.Id;
            this.client.Info = response.User;
            this.client.ConsoleStatePrefix = ConsoleMessages.UserPrefix + this.client.Info.Name;
        }
    }
}
