namespace EducationPortal.DAL.Entities.EF
{
    using System.Collections.Generic;

    public class Skill
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public IEnumerable<UserSkills> UserSkills { get; set; }

        public IEnumerable<Course> Courses { get; set; }
    }
}
