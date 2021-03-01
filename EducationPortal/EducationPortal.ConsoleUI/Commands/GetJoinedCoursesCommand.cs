namespace EducationPortal.ConsoleUI.Commands
{
    using System;
    using System.Linq;
    using EducationPortal.BLL.Services;
    using EducationPortal.ConsoleUI.Resources;
    using EducationPortal.ConsoleUI.Utilities;

    public class GetJoinedCoursesCommand : ICommand
    {
        private ClientData client;
        private IUserService userService;

        public GetJoinedCoursesCommand(IUserService userService, ClientData client)
        {
            this.client = client;
            this.userService = userService;
        }

        public string Name => "joinedcourses";

        public string Description => "joinedcourses\nОтобразить список курсов, в которых вы участвуете\n";

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

            var response = this.userService.GetJoinedCourses(this.client.Id);

            if (!response.IsSuccessful)
            {
                Console.WriteLine(OperationMessages.GetString(response.MessageCode));
                return;
            }

            var joinedCourses = response.Courses.ToArray();

            OutputHelper.PrintCourses(joinedCourses);
            this.client.CourseCache = joinedCourses;
        }
    }
}
