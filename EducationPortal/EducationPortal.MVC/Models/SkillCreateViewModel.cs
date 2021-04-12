namespace EducationPortal.MVC.Models
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using EducationPortal.BLL.DTO;

    public class SkillCreateViewModel
    {
        public string SkillName { get; set; }

        public long CourseId { get; set; }
    }
}
