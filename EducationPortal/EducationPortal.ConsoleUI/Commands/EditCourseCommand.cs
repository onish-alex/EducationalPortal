using System;
using System.Collections.Generic;
using System.Text;
using EducationPortal.BLL.DTO;
using EducationPortal.BLL.Response;
using EducationPortal.BLL.Services;
using EducationPortal.ConsoleUI.Validation;

namespace EducationPortal.ConsoleUI.Commands
{
    public class EditCourseCommand : ICommand<OperationResponse>
    {
        public OperationResponse Response { get; private set; }

        private ICourseService reciever;
        private CourseDTO course;
        private long userId;
        
        private CourseDataValidator validator;

        public EditCourseCommand(ICourseService reciever, CourseDTO course, long userId)
        {
            this.reciever = reciever;
            this.course = course;
            this.userId = userId;
            this.validator = new CourseDataValidator(course);
        }

        public void Execute()
        {
            var validationResult = validator.Validate();
            Response = (validationResult.IsValid) ? reciever.EditCourse(userId, course)
                                                  : new OperationResponse() { Message = validationResult.Message };
        }
    }
}
