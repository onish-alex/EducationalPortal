using EducationPortal.BLL.Response;
using EducationPortal.BLL.Services;
using System;
using System.Collections.Generic;
using System.Text;

namespace EducationPortal.ConsoleUI.Commands
{
    public class GetAllMaterialsCommand : ICommand<GetMaterialsResponse>
    {
        private IMaterialService materialService;

        public GetMaterialsResponse Response { get; set; }

        public GetAllMaterialsCommand(IMaterialService materialService)
        {
            this.materialService = materialService;
        }

        public void Execute()
        {
            Response = materialService.GetAllMaterials();
        }
    }
}
