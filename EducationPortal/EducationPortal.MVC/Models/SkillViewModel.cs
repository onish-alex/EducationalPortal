namespace EducationPortal.MVC.Models
{
    using System.Collections.Generic;
    using EducationPortal.BLL.DTO;

    public class SkillViewModel
    {
        public SkillDTO Skill { get; set; }

        public CourseDTO Course { get; set; }

        public IDictionary<long, bool> SkillsToRemove { get; set; }
    }
}
