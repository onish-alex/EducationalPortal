using System;
using System.Collections.Generic;
using System.Text;
using EducationPortal.ConsoleUI.Validation;
using EducationPortal.BLL.DTO;
using EducationPortal.BLL.Services;
using EducationPortal.BLL.Response;

namespace EducationPortal.ConsoleUI.Commands
{
    public class RemoveSkillCommand : ICommand<OperationResponse>
    {
        public OperationResponse Response { get; private set; }

        private ICourseService reciever;
        private long courseId;
        private long userId;
        private SkillDTO skill;

        public RemoveSkillCommand(ICourseService reciever, long userId, long courseId, SkillDTO skill)
        {
            this.reciever = reciever;
            this.courseId = courseId;
            this.userId = userId;
            this.skill = skill;
        }

        public void Execute()
        {
            Response = reciever.RemoveSkill(userId, courseId, skill);                                         
        }
    }
}
