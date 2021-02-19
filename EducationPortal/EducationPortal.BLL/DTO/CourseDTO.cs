namespace EducationPortal.BLL.DTO
{
    using System.Collections.Generic;

    public class CourseDTO
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public int CreatorId { get; set; }

        public IEnumerable<string> SkillNames { get; set; }
    }
}
