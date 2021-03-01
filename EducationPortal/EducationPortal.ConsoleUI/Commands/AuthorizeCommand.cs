namespace EducationPortal.ConsoleUI.Commands
{
    using EducationPortal.BLL.DTO;
    using EducationPortal.BLL.Response;
    using EducationPortal.BLL.Services;

    public class AuthorizeCommand : ICommand<AuthorizeResponse>
    {
        private IUserService reciever;
        private AccountDTO account;

        public AuthorizeCommand(IUserService reciever, AccountDTO account)
        {
            this.reciever = reciever;
            this.account = account;
        }

        public AuthorizeResponse Response { get; private set; }

        public void Execute()
        {
            this.Response = this.reciever.Authorize(this.account);
        }
    }
}
