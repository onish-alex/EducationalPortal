namespace EducationPortal.ConsoleUI.Commands
{
    using System;
    using EducationPortal.ConsoleUI.Resources;

    public class EnterCourseCommand : ICommand
    {
        private ClientData client;

        public EnterCourseCommand(ClientData client)
        {
            this.client = client;
        }

        public string Name => "entercourse";

        public string Description => "entercourse [number]\nВойти в выбранный курс\n";

        public int ParamsCount => 1;

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

            if (this.client.CourseCache == null)
            {
                Console.WriteLine(ConsoleMessages.ErrorCourseListIsEmpty);
                return;
            }

            var isNumberCorrect = long.TryParse(this.client.InputBuffer[0], out long number);

            if (!isNumberCorrect
             || number - 1 < 0
             || number - 1 >= this.client.CourseCache.Length)
            {
                Console.WriteLine(ConsoleMessages.ErrorIncorrectNumberOfCourse);
                return;
            }

            this.client.SelectedCourse = this.client.CourseCache[number - 1];
            this.client.CourseCache = null;
            this.client.ConsoleStatePrefix = ConsoleMessages.CoursePrefix + this.client.SelectedCourse.Name;
        }
    }
}
