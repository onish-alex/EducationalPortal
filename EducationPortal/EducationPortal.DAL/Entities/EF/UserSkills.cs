namespace EducationPortal.DAL.Entities.EF
{
    public class UserSkills
    {
        public long UserId { get; set; }

        public long SkillId { get; set; }

        public int Level { get; set; }

        public virtual User User { get; set; }

        public virtual Skill Skill { get; set; }
    }
}
