namespace EducationPortal.DAL.Repository.EF
{
    using EducationPortal.DAL.DbContexts;
    using EducationPortal.DAL.Entities.EF;

    public class EFCourseRepository : EFRepository<Course>
    {
        public EFCourseRepository(EFDbContext db)
            : base(db)
        {
        }
    }
}
