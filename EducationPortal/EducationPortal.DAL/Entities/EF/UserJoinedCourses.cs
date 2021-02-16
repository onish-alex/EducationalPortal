namespace EducationPortal.DAL.Entities.EF
{
    public class UserJoinedCourses
    {
        public int UserId { get; set; }

        public int CourseId { get; set; }

        public User User { get; set; }

        public Course Course { get; set; }
    }
}
