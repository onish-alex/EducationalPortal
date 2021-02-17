using EducationPortal.BLL.Response;
using EducationPortal.ConsoleUI.Validation;
using EducationPortal.BLL.DTO;
using EducationPortal.BLL.Services;

namespace EducationPortal.ConsoleUI.Commands
{
    public class AddCourseCommand : ICommand<OperationResponse>
    {
        public OperationResponse Response { get; private set; }

        private ICourseService reciever;
        private CourseDTO course;
        private CourseDataValidator validator;

        public AddCourseCommand(ICourseService reciever, CourseDTO course)
        {
            this.reciever = reciever;
            this.course = course;
            this.validator = new CourseDataValidator(course);
        }

        public void Execute()
        {
            var validationResult = validator.Validate();
            Response = (validationResult.IsValid) ? reciever.AddCourse(course)
                                                  : new OperationResponse() { Message = validationResult.Message };
        }
    }
}
