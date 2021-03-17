namespace EducationPortal.ConsoleUI.Commands
{
    using System;
    using System.Linq;
    using EducationPortal.BLL.Services;
    using EducationPortal.ConsoleUI.Resources;
    using EducationPortal.ConsoleUI.Utilities;

    public class GetAllCoursesCommand : ICommand
    {
        private ClientData client;
        private ICourseService courseService;

        public GetAllCoursesCommand(ICourseService courseService, ClientData client)
        {
            this.courseService = courseService;
            this.client = client;
        }

        public string Name => "allcourses";

        public string Description => "allcourses\nОтобразить список всех курсов\n";

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

            var response = this.courseService.GetAllCourses();

            var courseArray = response.Courses.ToArray();
            OutputHelper.PrintCourses(courseArray);
            this.client.CourseCache = courseArray;
        }
    }
}
