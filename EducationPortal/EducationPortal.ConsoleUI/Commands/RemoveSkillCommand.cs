namespace EducationPortal.ConsoleUI.Commands
{
    using System;
    using EducationPortal.BLL.DTO;
    using EducationPortal.BLL.Services;
    using EducationPortal.ConsoleUI.Resources;
    using EducationPortal.ConsoleUI.Utilities;

    public class RemoveSkillCommand : ICommand
    {
        private ICourseService courseService;
        private ClientData client;

        public RemoveSkillCommand(ICourseService courseService, ClientData client)
        {
            this.courseService = courseService;
            this.client = client;
        }

        public string Name => "removeskill";

        public string Description => "removeskill\nУдаление умения из выбранного курса\n";

        public int ParamsCount => 1;

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

            var checkResponse = this.courseService.CanEditCourse(this.client.Id, this.client.SelectedCourse.Id);

            if (!checkResponse.IsSuccessful)
            {
                Console.WriteLine(ResourceHelper.GetMessageString(checkResponse.MessageCode));
                return;
            }

            var skill = new SkillDTO()
            {
                Name = this.client.InputBuffer[0],
            };

            var removeSkillResponse = this.courseService.RemoveSkill(this.client.Id, this.client.SelectedCourse.Id, skill);
            Console.WriteLine(ResourceHelper.GetMessageString(removeSkillResponse.MessageCode));
        }
    }
}
