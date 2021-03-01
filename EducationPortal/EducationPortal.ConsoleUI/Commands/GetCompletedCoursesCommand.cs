namespace EducationPortal.ConsoleUI.Commands
{
    using System;
    using System.Linq;
    using EducationPortal.BLL.Services;
    using EducationPortal.ConsoleUI.Resources;
    using EducationPortal.ConsoleUI.Utilities;

    public class GetCompletedCoursesCommand : ICommand
    {
        private ClientData client;
        private IUserService userService;

        public GetCompletedCoursesCommand(IUserService userService, ClientData client)
        {
            this.client = client;
            this.userService = userService;
        }

        public string Name => "completedcourses";

        public string Description => "completedcourses\nОтобразить список завершенных вами курсов \n";

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

            var response = this.userService.GetCompletedCourses(this.client.Id);

            if (!response.IsSuccessful)
            {
                Console.WriteLine(OperationMessages.GetString(response.MessageCode));
                return;
            }

            var completedCourses = response.Courses.ToArray();

            OutputHelper.PrintCourses(completedCourses);
            this.client.CourseCache = completedCourses;
        }
    }
}
