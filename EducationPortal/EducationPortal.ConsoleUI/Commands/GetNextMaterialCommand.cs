namespace EducationPortal.ConsoleUI.Commands
{
    using EducationPortal.BLL.Response;
    using EducationPortal.BLL.Services;

    public class GetNextMaterialCommand : ICommand<GetMaterialsResponse>
    {
        private IUserService reciever;
        private int courseId;
        private int userId;

        public GetNextMaterialCommand(IUserService reciever, int userId, int courseId)
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
