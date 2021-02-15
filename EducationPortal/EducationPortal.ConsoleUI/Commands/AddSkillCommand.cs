using System;
using System.Collections.Generic;
using System.Text;
using EducationPortal.BLL.DTO;
using EducationPortal.BLL.Response;
using EducationPortal.BLL.Services;
using EducationPortal.ConsoleUI.Validation;

namespace EducationPortal.ConsoleUI.Commands
{
    public class AddSkillCommand : ICommand<OperationResponse>
    {
        public OperationResponse Response { get; private set; }

        private ICourseService reciever;
        private long courseId;
        private long userId;
        private SkillDTO skill;

        private SkillDataValidator validator;

        public AddSkillCommand(ICourseService reciever, long userId, long courseId, SkillDTO skill)
        {
            this.reciever = reciever;
            this.courseId = courseId;
            this.userId = userId;
            this.skill = skill;
            this.validator = new SkillDataValidator(skill);
        }

        public void Execute()
        {
            var validationResult = validator.Validate();
            Response = (validationResult.IsValid) ? reciever.AddSkill(userId, courseId, skill)
                                                  : new OperationResponse() { Message = validationResult.Message };
        }
    }
}
