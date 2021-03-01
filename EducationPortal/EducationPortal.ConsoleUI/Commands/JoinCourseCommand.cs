namespace EducationPortal.ConsoleUI.Commands
{
    using System;
    using EducationPortal.BLL.Services;
    using EducationPortal.ConsoleUI.Resources;

    public class JoinCourseCommand : ICommand
    {
        private ClientData client;
        private IUserService userService;
        private ICourseService courseService;

        public JoinCourseCommand(IUserService userService, ICourseService courseService, ClientData client)
        {
            this.client = client;
            this.userService = userService;
            this.courseService = courseService;
        }

        public string Name => "joincourse";

        public string Description => "joincourse\nНачать прохождение курса\n";

        public int ParamsCount => 0;

        public void Execute()
        {
            if (!this.client.IsAuthorized)
            {
                Console.WriteLine(ConsoleMessages.ErrorTryCommandWhileLoggedOut);
                return;
            }

            if (this.client.SelectedCourse == null)
            {
                Console.WriteLine(ConsoleMessages.ErrorNoSelectedCourse);
                return;
            }

            var joinCourseResponse = this.userService.JoinToCourse(this.client.Id, this.client.SelectedCourse.Id);

            Console.WriteLine(OperationMessages.GetString(joinCourseResponse.MessageCode));
        }
    }
}
