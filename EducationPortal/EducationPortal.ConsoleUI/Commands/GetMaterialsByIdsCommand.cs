using EducationPortal.BLL.Response;
using EducationPortal.BLL.Services;
using System;
using System.Collections.Generic;
using System.Text;

namespace EducationPortal.ConsoleUI.Commands
{
    public class GetMaterialsByIdsCommand : ICommand<GetMaterialsResponse>
    {
        private IMaterialService materialService;
        private long[] ids;

        public GetMaterialsResponse Response { get; set; }

        public GetMaterialsByIdsCommand(IMaterialService materialService, long[] ids)
        {
            this.materialService = materialService;
            this.ids = ids;
        }

        public void Execute()
        {
            Response = materialService.GetByIds(ids);
        }
    }
}
