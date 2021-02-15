using EducationPortal.BLL.Response;
using EducationPortal.BLL.Services;
using System;
using System.Collections.Generic;
using System.Text;

namespace EducationPortal.ConsoleUI.Commands
{
    public class CheckCourseEditingCommand : ICommand<OperationResponse>
    {
        public OperationResponse Response { get; private set; }

        private ICourseService reciever;
        private long userId;
        private long courseId;

        public CheckCourseEditingCommand(ICourseService reciever, long userId, long courseId)
        {
            this.reciever = reciever;
            this.courseId = courseId;
            this.userId = userId;
        }

        public void Execute()
        {
            Response = reciever.CanEditCourse(userId, courseId);
        }
    }
}
