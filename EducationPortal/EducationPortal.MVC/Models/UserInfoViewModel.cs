namespace EducationPortal.MVC.Models
{
    using System.Collections.Generic;
    using EducationPortal.BLL.DTO;

    public class UserInfoViewModel
    {
        public UserDTO User { get; set; }

        public Dictionary<long, CourseProgress> JoinedCoursesProgress { get; set; }

        public IEnumerable<CourseDTO> CompletedCourses { get; set; }

        public Dictionary<long, SkillLevel> SkillLevels { get; set; }

        public class CourseProgress
        {
            public CourseDTO Course { get; set; }

            public int ProgressPercent { get; set; }
        }

        public class SkillLevel
        {
            public SkillDTO Skill { get; set; }

            public int Level { get; set; }
        }
    }
}
