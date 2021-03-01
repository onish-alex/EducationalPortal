namespace EducationPortal.BLL.DTO
{
    public class UserSkillDTO
    {
        public SkillDTO Skill { get; set; }

        public int Level { get; set; }

        public override string ToString()
        {
            return string.Format("{0} ({1})", this.Skill.Name, this.Level);
        }
    }
}
