namespace EducationPortal.ConsoleUI.Commands
{
    using System;
    using EducationPortal.BLL.DTO;
    using EducationPortal.BLL.Services;
    using EducationPortal.BLL.Validation;
    using EducationPortal.ConsoleUI.Resources;
    using EducationPortal.ConsoleUI.Utilities;

    public class AddSkillCommand : ICommand
    {
        private ICourseService courseService;
        private ClientData client;
        private SkillValidator skillValidator;

        public AddSkillCommand(ICourseService courseService, SkillValidator skillValidator, ClientData client)
        {
            this.courseService = courseService;
            this.client = client;
            this.skillValidator = skillValidator;
        }

        public string Name => "addskill";

        public string Description => "addskill\nДобавление умения к выбранному курсу\n";

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
                Console.WriteLine(OperationMessages.GetString(checkResponse.MessageCode));
                return;
            }

            var skill = new SkillDTO()
            {
                Name = this.client.InputBuffer[0],
            };

            var validationResult = this.skillValidator.Validate(skill);
            if (!validationResult.IsValid)
            {
                var errorCode = validationResult.Errors[0].ErrorCode;
                OutputHelper.PrintValidationError(errorCode);
                return;
            }

            var addSkillResponse = this.courseService.AddSkill(this.client.Id, this.client.SelectedCourse.Id, skill);
            Console.WriteLine(OperationMessages.GetString(addSkillResponse.MessageCode));
        }
    }
}
