namespace EducationPortal.ConsoleUI.Commands
{
    using EducationPortal.BLL.Response;
    using EducationPortal.BLL.Services;

    public class CheckCourseEditingCommand : ICommand<OperationResponse>
    {
        private ICourseService reciever;
        private long userId;
        private long courseId;

        public CheckCourseEditingCommand(ICourseService reciever, long userId, long courseId)
        {
            this.reciever = reciever;
            this.courseId = courseId;
            this.userId = userId;
        }

        public OperationResponse Response { get; private set; }

        public void Execute()
        {
            this.Response = this.reciever.CanEditCourse(this.userId, this.courseId);
        }
    }
}
