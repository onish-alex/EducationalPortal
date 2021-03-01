namespace EducationPortal.BLL.Response
{
    using System.Collections.Generic;
    using EducationPortal.BLL.DTO;

    public class GetUserInfoResponse : OperationResponse
    {
        public UserDTO User { get; set; }

        public Dictionary<CourseDTO, int> JoinedCoursesProgress { get; set; }

        public IEnumerable<CourseDTO> CompletedCourses { get; set; }

        public Dictionary<SkillDTO, int> SkillLevels { get; set; }
    }
}
