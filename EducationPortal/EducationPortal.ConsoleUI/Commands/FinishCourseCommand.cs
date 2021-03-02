namespace EducationPortal.ConsoleUI.Commands
{
    using System;
    using EducationPortal.BLL.Services;
    using EducationPortal.ConsoleUI.Resources;
    using EducationPortal.ConsoleUI.Utilities;

    public class FinishCourseCommand : ICommand
    {
        private ClientData client;
        private IUserService userService;

        public FinishCourseCommand(IUserService userService, ClientData client)
        {
            this.client = client;
            this.userService = userService;
        }

        public string Name => "finish";

        public string Description => "finish\nПодтвердить завершение курса\n";

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

            var completeCourseResponse = this.userService.AddCompletedCourse(this.client.Id, this.client.SelectedCourse.Id);

            if (!completeCourseResponse.IsSuccessful)
            {
                Console.WriteLine(ResourceHelper.GetMessageString(completeCourseResponse.MessageCode));
                return;
            }

            Console.WriteLine(ConsoleMessages.OutputCourseCompleted, this.client.SelectedCourse.Name);

            foreach (var skillLevelPair in completeCourseResponse.RecievedSkills)
            {
                if (skillLevelPair.Value == 1)
                {
                    Console.WriteLine(ConsoleMessages.OutputNewSkillRecieved, skillLevelPair.Key.Name);
                }
                else
                {
                    Console.WriteLine(ConsoleMessages.OutputSkillIncreased, skillLevelPair.Key.Name);
                }
            }
        }
    }
}
