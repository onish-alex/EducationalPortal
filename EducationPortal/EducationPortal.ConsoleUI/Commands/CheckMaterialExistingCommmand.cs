namespace EducationPortal.ConsoleUI.Commands
{
    using EducationPortal.BLL.Response;
    using EducationPortal.BLL.Services;

    public class CheckMaterialExistingCommmand : ICommand<OperationResponse>
    {
        private IMaterialService reciever;
        private long materialId;

        public CheckMaterialExistingCommmand(IMaterialService reciever, long materialId)
        {
            this.reciever = reciever;
            this.materialId = materialId;
        }

        public OperationResponse Response { get; private set; }

        public void Execute()
        {
            this.Response = this.reciever.CheckMaterialExisting(this.materialId);
        }
    }
}
