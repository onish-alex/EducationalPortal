namespace EducationPortal.ConsoleUI.Commands
{
    using System;
    using System.Linq;
    using EducationPortal.BLL.Services;
    using EducationPortal.ConsoleUI.Resources;

    public class GetCourseInfoCommand : ICommand
    {
        private ClientData client;
        private ICourseService courseService;

        public GetCourseInfoCommand(ICourseService courseService, ClientData client)
        {
            this.client = client;
            this.courseService = courseService;
        }

        public string Name => "courseinfo";

        public string Description => "courseinfo\nОтобразить информацию о выбранном курсе\n";

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

            var getCourseStatusResponse = this.courseService.GetCourseStatus(this.client.SelectedCourse.Id, this.client.Id);

            Console.WriteLine(ConsoleMessages.OutputCourseName, this.client.SelectedCourse.Name);
            Console.WriteLine(ConsoleMessages.OutputCourseAuthorName, getCourseStatusResponse.CreatorName);
            Console.WriteLine(ConsoleMessages.OutputCourseDescription, this.client.SelectedCourse.Description);

            var skillStr = getCourseStatusResponse.Skills.Count() != 0
                                ? string.Join(", ", getCourseStatusResponse.Skills.Select(a => a.Name))
                                : ConsoleMessages.OutputEmptySkillList;

            Console.WriteLine(ConsoleMessages.OutputCourseSkills, skillStr);

            if (!getCourseStatusResponse.IsSuccessful)
            {
                Console.WriteLine(OperationMessages.GetString(getCourseStatusResponse.MessageCode));
                return;
            }

            if (getCourseStatusResponse.IsCreator)
            {
                Console.WriteLine(ConsoleMessages.OutputIsCourseAuthor);
            }

            if (getCourseStatusResponse.IsJoined)
            {
                Console.WriteLine(ConsoleMessages.OutputIsJoiningCourse);
            }

            if (getCourseStatusResponse.IsCompleted)
            {
                Console.WriteLine(ConsoleMessages.OutputHasCompleteCourse);
            }
        }
    }
}
