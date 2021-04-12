namespace EducationPortal.BLL.DTO
{
    using System.Collections.Generic;

    public class CourseDTO
    {
        public long Id { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public long CreatorId { get; set; }

        public IList<SkillDTO> Skills { get; set; }
    }
}
