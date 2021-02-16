namespace EducationPortal.ConsoleUI.Commands
{
    using EducationPortal.BLL.Response;
    using EducationPortal.BLL.Services;

    public class GetCourseStatusCommand : ICommand<GetCourseStatusResponse>
    {
        private ICourseService reciever;
        private long userId;
        private long courseId;

        public GetCourseStatusCommand(ICourseService reciever, long userId, long courseId)
        {
            this.reciever = reciever;
            this.userId = userId;
            this.courseId = courseId;
        }

        public GetCourseStatusResponse Response { get; private set; }

        public void Execute()
        {
            this.Response = this.reciever.GetCourseStatus(this.courseId, this.userId);
        }
    }
}
