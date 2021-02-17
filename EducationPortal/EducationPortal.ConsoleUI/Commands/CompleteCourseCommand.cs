using EducationPortal.BLL.DTO;
using EducationPortal.BLL.Response;
using EducationPortal.BLL.Services;
using System;
using System.Collections.Generic;
using System.Text;

namespace EducationPortal.ConsoleUI.Commands
{
    public class CompleteCourseCommand : ICommand<OperationResponse>
    {
        public OperationResponse Response { get; private set; }

        private IUserService reciever;
        private long userId;
        private CourseDTO course;

        public CompleteCourseCommand(IUserService reciever, long userId, CourseDTO course)
        {
            this.reciever = reciever;
            this.course = course;
            this.userId = userId;
        }

        public void Execute()
        {
            Response = reciever.AddCompletedCourse(userId, course);
        }
    }
}
