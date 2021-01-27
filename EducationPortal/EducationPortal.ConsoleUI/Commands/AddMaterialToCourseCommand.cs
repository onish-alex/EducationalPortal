using System;
using System.Collections.Generic;
using System.Text;
using EducationPortal.BLL.Response;
using EducationPortal.ConsoleUI.Validation;
using EducationPortal.BLL.DTO;
using EducationPortal.BLL.Services;

namespace EducationPortal.ConsoleUI.Commands
{
    public class AddMaterialToCourseCommand : ICommand<OperationResponse>
    {
        public OperationResponse Response { get; private set; }

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

        public void Execute()
        {
            Response = reciever.AddMaterialToCourse(userId, courseId, materialId);
        }
    }
}
