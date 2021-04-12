namespace EducationPortal.MVC.Models
{
    using System.Collections.Generic;
    using EducationPortal.BLL.DTO;

    public class CourseCompleteViewModel
    {
        public IDictionary<SkillDTO, int> RecievedSkills { get; set; }

        public string CourseName { get; set; }

        public long CourseId { get; set; }
    }
}
