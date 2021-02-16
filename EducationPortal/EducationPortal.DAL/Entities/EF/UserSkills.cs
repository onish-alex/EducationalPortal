namespace EducationPortal.DAL.Entities.EF
{
    public class UserSkills
    {
        public int UserId { get; set; }

        public int SkillId { get; set; }

        public int Level { get; set; }

        public User User { get; set; }

        public Skill Skill { get; set; }
    }
}
