namespace EducationPortal.DAL.Repository.EF
{
    using System.Linq;
    using EducationPortal.DAL.DbContexts;
    using EducationPortal.DAL.Entities.EF;
    using Microsoft.EntityFrameworkCore;

    public class CourseRepository : EFRepository<Course>
    {
        public CourseRepository(EFDbContext db)
            : base(db)
        {
        }

        protected override IQueryable<Course> SelectQuery => this.db.Set<Course>()
                                                                    .Include(c => c.Materials)
                                                                    .Include(c => c.Skills)
                                                                    .Include(c => c.Creator)
                                                                    .Include(c => c.CompletedUsers)
                                                                    .Include(c => c.JoinedUsers);

        public override Course GetById(long id)
        {
            return this.SelectQuery.SingleOrDefault(c => c.Id == id);
        }
    }
}
