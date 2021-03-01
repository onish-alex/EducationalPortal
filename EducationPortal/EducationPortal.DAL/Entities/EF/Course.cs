namespace EducationPortal.DAL.Entities.EF
{
    using System.Collections.Generic;

    public class Course
    {
        public long Id { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public long CreatorId { get; set; }

        public virtual User Creator { get; set; }

        public virtual ICollection<Material> Materials { get; set; }

        public virtual ICollection<Skill> Skills { get; set; }

        public virtual ICollection<UserCompletedCourses> CompletedUsers { get; set; }

        public virtual ICollection<UserJoinedCourses> JoinedUsers { get; set; }
    }
}
