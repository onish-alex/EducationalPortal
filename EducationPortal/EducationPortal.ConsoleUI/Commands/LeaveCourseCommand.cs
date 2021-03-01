namespace EducationPortal.ConsoleUI.Commands
{
    using System;
    using EducationPortal.ConsoleUI.Resources;

    public class LeaveCourseCommand : ICommand
    {
        private ClientData client;

        public LeaveCourseCommand(ClientData client)
        {
            this.client = client;
        }

        public string Name => "leavecourse";

        public string Description => "leavecourse\nВыйти из выбранного курса\n";

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

            this.client.SelectedCourse = null;
            this.client.ConsoleStatePrefix = ConsoleMessages.UserPrefix + this.client.Info.Name;
        }
    }
}
