namespace EducationPortal.ConsoleUI.Commands
{
    using EducationPortal.BLL.Response;
    using EducationPortal.BLL.Services;

    public class CompleteCourseCommand : ICommand<CompletedCourseResponse>
    {
        private IUserService reciever;
        private long userId;
        private long courseId;

        public CompleteCourseCommand(IUserService reciever, long userId, long courseId)
        {
            this.reciever = reciever;
            this.courseId = courseId;
            this.userId = userId;
        }

        public CompletedCourseResponse Response { get; private set; }

        public void Execute()
        {
            this.Response = this.reciever.AddCompletedCourse(this.userId, this.courseId);
        }
    }
}
