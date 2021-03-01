namespace EducationPortal.DAL.Entities.EF
{
    using System.Collections.Generic;

    public class Skill
    {
        public long Id { get; set; }

        public string Name { get; set; }

        public virtual IEnumerable<UserSkills> UserSkills { get; set; }

        public virtual IEnumerable<Course> Courses { get; set; }
    }
}
