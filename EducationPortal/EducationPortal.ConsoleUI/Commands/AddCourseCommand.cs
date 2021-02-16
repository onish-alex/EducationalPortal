namespace EducationPortal.ConsoleUI.Commands
{
    using EducationPortal.BLL.DTO;
    using EducationPortal.BLL.Response;
    using EducationPortal.BLL.Services;
    using EducationPortal.ConsoleUI.Validation;

    public class AddCourseCommand : ICommand<OperationResponse>
    {
        private ICourseService reciever;
        private CourseDTO course;
        private CourseDataValidator validator;

        public AddCourseCommand(ICourseService reciever, CourseDTO course)
        {
            this.reciever = reciever;
            this.course = course;
            this.validator = new CourseDataValidator(course);
        }

        public OperationResponse Response { get; private set; }

        public void Execute()
        {
            var validationResult = this.validator.Validate();
            this.Response = validationResult.IsValid ? this.reciever.AddCourse(this.course)
                                                     : new OperationResponse() { Message = validationResult.Message };
        }
    }
}
