namespace EducationPortal.ConsoleUI.Commands
{
    using EducationPortal.BLL.Response;
    using EducationPortal.BLL.Services;

    public class GetNextMaterialCommand : ICommand<GetMaterialsResponse>
    {
        private IUserService reciever;
        private long courseId;
        private long userId;

        public GetNextMaterialCommand(IUserService reciever, long userId, long courseId)
        {
            this.reciever = reciever;
            this.courseId = courseId;
            this.userId = userId;
        }

        public GetMaterialsResponse Response { get; private set; }

        public void Execute()
        {
            this.Response = this.reciever.GetNextMaterial(this.userId, this.courseId);
        }
    }
}
