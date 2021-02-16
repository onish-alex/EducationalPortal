namespace EducationPortal.ConsoleUI.Commands
{
    using EducationPortal.BLL.DTO;
    using EducationPortal.BLL.Response;
    using EducationPortal.BLL.Services;
    using EducationPortal.ConsoleUI.Validation;

    public class EditCourseCommand : ICommand<OperationResponse>
    {
        private ICourseService reciever;
        private CourseDTO course;
        private long userId;
        private CourseDataValidator validator;

        public EditCourseCommand(ICourseService reciever, CourseDTO course, long userId)
        {
            this.reciever = reciever;
            this.course = course;
            this.userId = userId;
            this.validator = new CourseDataValidator(course);
        }

        public OperationResponse Response { get; private set; }

        public void Execute()
        {
            var validationResult = this.validator.Validate();
            this.Response = validationResult.IsValid ? this.reciever.EditCourse(this.userId, this.course)
                                                     : new OperationResponse() { Message = validationResult.Message };
        }
    }
}
