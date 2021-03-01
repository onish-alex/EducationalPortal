namespace EducationPortal.ConsoleUI.Commands
{
    using System;
    using EducationPortal.ConsoleUI.Resources;

    public class DeauthorizeCommand : ICommand
    {
        private ClientData client;

        public DeauthorizeCommand(ClientData client)
        {
            this.client = client;
        }

        public string Name => "logout";

        public string Description => "logout\nВыход из системы\n";

        public int ParamsCount => 0;

        public void Execute()
        {
            if (this.client.IsAuthorized)
            {
                this.client.IsAuthorized = false;
                this.client.SelectedCourse = null;
                this.client.CourseCache = null;
                this.client.ConsoleStatePrefix = ConsoleMessages.DefaultCommandPrefix;
                Console.WriteLine(ConsoleMessages.InfoLoggedOut);
            }
            else
            {
                Console.WriteLine(ConsoleMessages.ErrorTryLogOutWhileLoggedOut);
            }
        }
    }
}
