namespace EducationPortal.DAL.Entities.EF
{
    using System.Collections.Generic;

    public class Course
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public int CreatorId { get; set; }

        public User Creator { get; set; }

        public ICollection<Material> Materials { get; set; }

        public ICollection<Skill> Skills { get; set; }

        public ICollection<UserCompletedCourses> CompletedUsers { get; set; }

        public ICollection<UserJoinedCourses> JoinedUsers { get; set; }
    }
}
