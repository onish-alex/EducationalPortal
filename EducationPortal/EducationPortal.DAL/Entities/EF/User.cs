namespace EducationPortal.DAL.Entities.EF
{
    using System.Collections.Generic;

    public class User
    {
        public long Id { get; set; }

        public string Name { get; set; }

        public virtual ICollection<UserJoinedCourses> JoinedCourses { get; set; }

        public virtual ICollection<UserCompletedCourses> CompletedCourses { get; set; }

        public virtual ICollection<Material> LearnedMaterials { get; set; }

        public virtual ICollection<Course> CreatedCourses { get; set; }

        public virtual ICollection<UserSkills> UserSkills { get; set; }

        public virtual Account Account { get; set; }
    }
}
