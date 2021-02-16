namespace EducationPortal.ConsoleUI.Commands
{
    using EducationPortal.BLL.Response;
    using EducationPortal.BLL.Services;

    public class AddMaterialToCourseCommand : ICommand<OperationResponse>
    {
        private ICourseService reciever;
        private long materialId;
        private long userId;
        private long courseId;

        public AddMaterialToCourseCommand(ICourseService reciever, long userId, long courseId, long materialId)
        {
            this.reciever = reciever;
            this.userId = userId;
            this.courseId = courseId;
            this.materialId = materialId;
        }

        public OperationResponse Response { get; private set; }

        public void Execute()
        {
            this.Response = this.reciever.AddMaterialToCourse(this.userId, this.courseId, this.materialId);
        }
    }
}
