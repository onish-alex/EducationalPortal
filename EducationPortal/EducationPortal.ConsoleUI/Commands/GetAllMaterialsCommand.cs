namespace EducationPortal.ConsoleUI.Commands
{
    using EducationPortal.BLL.Response;
    using EducationPortal.BLL.Services;

    public class GetAllMaterialsCommand : ICommand<GetMaterialsResponse>
    {
        private IMaterialService materialService;

        public GetAllMaterialsCommand(IMaterialService materialService)
        {
            this.materialService = materialService;
        }

        public GetMaterialsResponse Response { get; set; }

        public void Execute()
        {
            this.Response = this.materialService.GetAllMaterials();
        }
    }
}
