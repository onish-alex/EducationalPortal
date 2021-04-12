namespace EducationPortal.BLL.Results
{
    using System.Collections.Generic;
    using EducationPortal.BLL.DTO;

    public class GetUserInfoResult : OperationResult
    {
        public UserDTO User { get; set; }

        public Dictionary<CourseDTO, int> JoinedCoursesProgress { get; set; }

        public IEnumerable<CourseDTO> CompletedCourses { get; set; }

        public Dictionary<SkillDTO, int> SkillLevels { get; set; }
    }
}
