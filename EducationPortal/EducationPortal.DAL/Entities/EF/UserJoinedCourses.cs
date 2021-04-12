namespace EducationPortal.DAL.Entities.EF
{
    public class UserJoinedCourses
    {
        public long UserId { get; set; }

        public long CourseId { get; set; }

        public virtual User User { get; set; }

        public virtual Course Course { get; set; }
    }
}
