using System;
using System.Collections.Generic;
using System.Text;

namespace EducationPortal.BLL.DTO
{
    public class CourseDTO
    {
        public long Id { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public long CreatorId { get; set; }

        public SkillDTO[] Skills { get; set; }

        public long[] MaterialIds { get; set; }
    }
}
