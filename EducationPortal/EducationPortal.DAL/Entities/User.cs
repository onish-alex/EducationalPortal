using System.Collections.Generic;

namespace EducationPortal.DAL.Entities
{
    public class User : Entity
    {
        public string Name { get; set; }

        public long[] CompletedCourseIds { get; set; }

        public long[] JoinedCourseIds { get; set; }

        public long[] CompletedMaterialIds { get; set; }

        public Dictionary<long, int> Skills { get; set; }  

    }
}
