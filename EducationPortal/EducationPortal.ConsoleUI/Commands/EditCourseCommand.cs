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
        private IValidator validator;

        public EditCourseCommand(ICourseService reciever, IValidator validator, CourseDTO course, long userId)
        {
            this.reciever = reciever;
            this.course = course;
            this.userId = userId;
            this.validator = validator;
        }

        public OperationResponse Response { get; private set; }

        public void Execute()
        {
            this.validator.SetData(this.course);
            var validationResult = this.validator.Validate();
            this.Response = validationResult.IsValid ? this.reciever.EditCourse(this.userId, this.course)
                                                     : new OperationResponse() { Message = validationResult.Message };
        }
    }
}
