namespace EducationPortal.ConsoleUI.Commands
{
    using EducationPortal.BLL.DTO;
    using EducationPortal.BLL.Response;
    using EducationPortal.BLL.Services;
    using EducationPortal.ConsoleUI.Validation;

    public class AddSkillCommand : ICommand<OperationResponse>
    {
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

        public OperationResponse Response { get; private set; }

        public void Execute()
        {
            var validationResult = this.validator.Validate();
            this.Response = validationResult.IsValid ? this.reciever.AddSkill(this.userId, this.courseId, this.skill)
                                                     : new OperationResponse() { Message = validationResult.Message };
        }
    }
}
