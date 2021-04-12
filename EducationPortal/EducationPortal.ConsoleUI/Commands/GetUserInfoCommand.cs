namespace EducationPortal.ConsoleUI.Commands
{
    using System;
    using System.Linq;
    using EducationPortal.BLL.Services;
    using EducationPortal.ConsoleUI.Resources;
    using EducationPortal.ConsoleUI.Utilities;

    public class GetUserInfoCommand : ICommand
    {
        private ClientData client;
        private IUserService userService;

        public GetUserInfoCommand(IUserService userService, ClientData client)
        {
            this.client = client;
            this.userService = userService;
        }

        public string Name => "myinfo";

        public string Description => "myinfo\nОтобразить личную информацию\n";

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

            var response = this.userService.GetUserById(this.client.Id);

            if (!response.IsSuccessful)
            {
                Console.WriteLine(ResourceHelper.GetMessageString(response.MessageCode));
                return;
            }

            Console.WriteLine(ConsoleMessages.OutputProfileUserName, this.client.Info.Name);

            var userSkillStrings = response.SkillLevels.Select(sl => string.Format("{0} ({1})", sl.Key.Name, sl.Value));
            Console.WriteLine(ConsoleMessages.OutputProfileSkills, string.Join(", ", userSkillStrings));

            var joinedCourseStrings = response.JoinedCoursesProgress.Select(cp => string.Format("\n{0} - {1}%", cp.Key.Name, cp.Value));
            Console.WriteLine(ConsoleMessages.OutputProfileJoinedCourses, string.Join(", ", joinedCourseStrings));

            var courseNameStrings = response.CompletedCourses.Select(cc => cc.Name);
            Console.WriteLine(ConsoleMessages.OutputProfileCompletedCourses, string.Join(", ", courseNameStrings));
        }
    }
}
