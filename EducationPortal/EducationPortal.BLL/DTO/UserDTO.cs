using System.Collections.Generic;

namespace EducationPortal.BLL.DTO
{
    public class UserDTO
    {
        public string Name { get; set; }

        public long[] CompletedCourseIds { get; set; }

        public long[] JoinedCourseIds { get; set; }

        public long[] CompletedMaterialIds { get; set; }

        public Dictionary<string, UserSkillDTO> Skills { get; set; }
    }
}
