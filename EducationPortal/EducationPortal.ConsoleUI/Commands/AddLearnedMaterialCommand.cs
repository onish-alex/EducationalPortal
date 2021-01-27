using EducationPortal.BLL.Response;
using EducationPortal.BLL.Services;
using System;
using System.Collections.Generic;
using System.Text;

namespace EducationPortal.ConsoleUI.Commands
{
    public class AddLearnedMaterialCommand : ICommand<OperationResponse>
    {
        private IUserService userService;

        public OperationResponse Response { get; set; }

        private long userId;
        private long materialId;

        public AddLearnedMaterialCommand(IUserService userService, long userId, long materialId)
        {
            this.userService = userService;
            this.userId = userId;
            this.materialId = materialId;
        }

        public void Execute()
        {
            Response = userService.AddLearnedMaterial(userId, materialId);
        }
    }
}
