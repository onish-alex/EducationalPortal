namespace EducationPortal.ConsoleUI.Commands
{
    using System;
    using EducationPortal.BLL.DTO;
    using EducationPortal.BLL.Services;
    using EducationPortal.BLL.Validation;
    using EducationPortal.ConsoleUI.Resources;
    using EducationPortal.ConsoleUI.Utilities;

    public class AddCourseCommand : ICommand
    {
        private ICourseService courseService;
        private ClientData client;
        private IValidator<CourseDTO> courseValidator;

        public AddCourseCommand(ICourseService courseService, IValidator<CourseDTO> courseValidator, ClientData client)
        {
            this.courseService = courseService;
            this.client = client;
            this.courseValidator = courseValidator;
        }

        public string Name => "createcourse";

        public string Description => "createcourse\nСоздание нового курса\n";

        public int ParamsCount => 0;

        public void Execute()
        {
            if (!this.client.IsAuthorized)
            {
                Console.WriteLine(ConsoleMessages.ErrorTryCommandWhileLoggedOut);
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
                CreatorId = this.client.Id,
            };

            var validationResult = this.courseValidator.Validate(course);

            if (!validationResult.IsValid)
            {
                var errorCode = validationResult.Errors[0].ErrorCode;
                OutputHelper.PrintValidationError(errorCode);
                return;
            }

            var response = this.courseService.AddCourse(course);
            Console.WriteLine(ResourceHelper.GetMessageString(response.MessageCode));
        }
    }
}
