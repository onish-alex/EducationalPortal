namespace EducationPortal.ConsoleUI.Commands
{
    using System;
    using EducationPortal.BLL.DTO;
    using EducationPortal.BLL.Services;
    using EducationPortal.BLL.Validation;
    using EducationPortal.ConsoleUI.Resources;
    using EducationPortal.ConsoleUI.Utilities;

    public class EditCourseCommand : ICommand
    {
        private ICourseService courseService;
        private ClientData client;
        private CourseValidator courseValidator;

        public EditCourseCommand(ICourseService courseService, CourseValidator courseValidator, ClientData client)
        {
            this.courseService = courseService;
            this.client = client;
            this.courseValidator = courseValidator;
        }

        public string Name => "editcourse";

        public string Description => "editcourse\nРедактирование выбранного курса\n";

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

            var checkResponse = this.courseService.CanEditCourse(this.client.Id, this.client.SelectedCourse.Id);

            if (!checkResponse.IsSuccessful)
            {
                Console.WriteLine(OperationMessages.GetString(checkResponse.MessageCode));
                return;
            }

            Console.WriteLine(ConsoleMessages.InputCourseName);
            var name = Console.ReadLine();
            Console.WriteLine(ConsoleMessages.InputCourseDescription);
            var description = Console.ReadLine();

            var course = new CourseDTO()
            {
                Name = name,
                Description = description,
                CreatorId = this.client.SelectedCourse.CreatorId,
                Id = this.client.SelectedCourse.Id,
            };

            var validationResult = this.courseValidator.Validate(course);

            if (!validationResult.IsValid)
            {
                var errorCode = validationResult.Errors[0].ErrorCode;
                OutputHelper.PrintValidationError(errorCode);
                return;
            }

            var response = this.courseService.EditCourse(this.client.Id, course);
            Console.WriteLine(OperationMessages.GetString(response.MessageCode));

            if (response.IsSuccessful)
            {
                this.client.ConsoleStatePrefix = ConsoleMessages.CoursePrefix + name;
                this.client.SelectedCourse.Name = name;
                this.client.SelectedCourse.Description = description;
            }
        }
    }
}
