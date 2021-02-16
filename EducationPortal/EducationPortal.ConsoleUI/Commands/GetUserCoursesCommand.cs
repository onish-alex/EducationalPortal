namespace EducationPortal.ConsoleUI.Commands
{
    using EducationPortal.BLL.Response;
    using EducationPortal.BLL.Services;

    public class GetUserCoursesCommand : ICommand<GetCoursesResponse>
    {
        private ICourseService courseService;
        private long userId;

        public GetUserCoursesCommand(ICourseService courseService, long userId)
        {
            this.courseService = courseService;
            this.userId = userId;
        }

        public GetCoursesResponse Response { get; set; }

        public void Execute()
        {
            this.Response = this.courseService.GetUserCourses(this.userId);
        }
    }
}
