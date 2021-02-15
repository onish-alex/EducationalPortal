using EducationPortal.BLL.Response;
using EducationPortal.BLL.Services;
using System;
using System.Collections.Generic;
using System.Text;

namespace EducationPortal.ConsoleUI.Commands
{
    public class CheckCourseJoiningCommand : ICommand<OperationResponse>
    {
        public OperationResponse Response { get; private set; }

        private ICourseService reciever;
        private long userId;
        private long courseId;

        public CheckCourseJoiningCommand(ICourseService reciever, long userId, long courseId)
        {
            this.reciever = reciever;
            this.userId = userId;
            this.courseId = courseId;
        }

        public void Execute()
        {
            Response = reciever.CanJoinCourse(userId, courseId);
        }
    }
}
