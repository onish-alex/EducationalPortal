namespace EducationPortal.ConsoleUI.Commands
{
    using EducationPortal.BLL.Response;
    using EducationPortal.BLL.Services;

    public class CheckCourseJoiningCommand : ICommand<OperationResponse>
    {
        private ICourseService reciever;
        private long userId;
        private long courseId;

        public CheckCourseJoiningCommand(ICourseService reciever, long userId, long courseId)
        {
            this.reciever = reciever;
            this.userId = userId;
            this.courseId = courseId;
        }

        public OperationResponse Response { get; private set; }

        public void Execute()
        {
            this.Response = this.reciever.CanJoinCourse(this.userId, this.courseId);
        }
    }
}
