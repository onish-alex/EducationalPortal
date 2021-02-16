namespace EducationPortal.ConsoleUI.Commands
{
    using EducationPortal.BLL.Response;
    using EducationPortal.BLL.Services;

    public class GetAllCoursesCommand : ICommand<GetCoursesResponse>
    {
        private ICourseService courseService;

        public GetAllCoursesCommand(ICourseService courseService)
        {
            this.courseService = courseService;
        }

        public GetCoursesResponse Response { get; set; }

        public void Execute()
        {
            this.Response = this.courseService.GetAllCourses();
        }
    }
}
