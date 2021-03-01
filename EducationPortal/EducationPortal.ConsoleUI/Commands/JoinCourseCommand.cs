namespace EducationPortal.ConsoleUI.Commands
{
    using EducationPortal.BLL.Response;
    using EducationPortal.BLL.Services;

    public class JoinCourseCommand : ICommand<OperationResponse>
    {
        private IUserService reciever;
        private long userId;
        private long courseId;

        public JoinCourseCommand(IUserService reciever, long userId, long courseId)
        {
            this.reciever = reciever;
            this.courseId = courseId;
            this.userId = userId;
        }

        public OperationResponse Response { get; private set; }

        public void Execute()
        {
            this.Response = this.reciever.JoinToCourse(this.userId, this.courseId);
        }
    }
}
