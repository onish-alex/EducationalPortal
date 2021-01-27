namespace EducationPortal.DAL.Entities
{
    public class Course : Entity
    {
        public string Name { get; set; }

        public string Description { get; set; }

        public long CreatorId { get; set; }

        public long[] MaterialIds { get; set; }

        public long[] SkillIds { get; set; }

    }
}
