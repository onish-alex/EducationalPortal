namespace EducationPortal.ConsoleUI.Commands
{
    using System;
    using System.Linq;
    using EducationPortal.BLL.Services;
    using EducationPortal.ConsoleUI.Resources;
    using EducationPortal.ConsoleUI.Utilities;

    public class GetUserCoursesCommand : ICommand
    {
        private ClientData client;
        private ICourseService courseService;

        public GetUserCoursesCommand(ICourseService courseService, ClientData client)
        {
            this.courseService = courseService;
            this.client = client;
        }

        public string Name => "mycourses";

        public string Description => "mycourses\nОтобразить список созданных вами курсов\n";

        public int ParamsCount => 0;

        public void Execute()
        {
            if (!this.client.IsAuthorized)
            {
                Console.WriteLine(ConsoleMessages.ErrorTryCommandWhileLoggedOut);
                return;
            }

            if (this.client.SelectedCourse != null)
            {
                Console.WriteLine(ConsoleMessages.ErrorAlreadyInCourseMode);
                return;
            }

            var response = this.courseService.GetUserCourses(this.client.Id);

            var courseArray = response.Courses.ToArray();
            this.client.CourseCache = courseArray;
            OutputHelper.PrintCourses(courseArray);
        }
    }
}
