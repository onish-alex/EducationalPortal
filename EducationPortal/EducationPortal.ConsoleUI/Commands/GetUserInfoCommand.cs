namespace EducationPortal.ConsoleUI.Commands
{
    using EducationPortal.BLL.Response;
    using EducationPortal.BLL.Services;

    public class GetUserInfoCommand : ICommand<GetUserInfoResponse>
    {
        private IUserService userService;

        private long userId;

        public GetUserInfoCommand(IUserService userService, long userId)
        {
            this.userService = userService;
            this.userId = userId;
        }

        public GetUserInfoResponse Response { get; set; }

        public void Execute()
        {
            this.Response = this.userService.GetUserById(this.userId);
        }
    }
}
