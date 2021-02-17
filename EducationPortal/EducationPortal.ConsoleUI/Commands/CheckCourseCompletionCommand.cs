using EducationPortal.BLL.Response;
using EducationPortal.BLL.Services;
using System;
using System.Collections.Generic;
using System.Text;

namespace EducationPortal.ConsoleUI.Commands
{
    class CheckCourseCompletionCommand : ICommand<OperationResponse>
    {
        public OperationResponse Response { get; private set; }

        private ICourseService reciever;
        private long[] learnedMaterialIds;
        private long courseId;

        public CheckCourseCompletionCommand(ICourseService reciever, long courseId, long[] learnedMaterialIds)
        {
            this.reciever = reciever;
            this.learnedMaterialIds = learnedMaterialIds;
            this.courseId = courseId;
        }

        public void Execute()
        {
            Response = reciever.CanCompleteCourse(courseId, learnedMaterialIds);
        }
    }
}
