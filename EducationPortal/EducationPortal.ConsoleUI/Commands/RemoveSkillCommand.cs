namespace EducationPortal.ConsoleUI.Commands
{
    using EducationPortal.BLL.DTO;
    using EducationPortal.BLL.Response;
    using EducationPortal.BLL.Services;

    public class RemoveSkillCommand : ICommand<OperationResponse>
    {
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

        public OperationResponse Response { get; private set; }

        public void Execute()
        {
            this.Response = this.reciever.RemoveSkill(this.userId, this.courseId, this.skill);
        }
    }
}
