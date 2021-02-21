namespace EducationPortal.DAL.Repository.EF
{
    using EducationPortal.DAL.DbContexts;
    using EducationPortal.DAL.Entities.EF;

    public class CourseRepository : EFRepository<Course>
    {
        public CourseRepository(EFDbContext db)
            : base(db)
        {
        }
    }
}
