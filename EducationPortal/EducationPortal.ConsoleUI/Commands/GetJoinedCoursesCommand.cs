namespace EducationPortal.ConsoleUI.Commands
{
    using EducationPortal.BLL.Response;
    using EducationPortal.BLL.Services;

    public class GetJoinedCoursesCommand : ICommand<GetCoursesResponse>
    {
        private IUserService userService;
        private long userId;

        public GetJoinedCoursesCommand(IUserService userService, long userId)
        {
            this.userService = userService;
            this.userId = userId;
        }

        public GetCoursesResponse Response { get; set; }

        public void Execute()
        {
            this.Response = this.userService.GetJoinedCourses(this.userId);
        }
    }
}
