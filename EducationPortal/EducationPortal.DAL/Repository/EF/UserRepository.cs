namespace EducationPortal.DAL.Repository.EF
{
    using System.Linq;
    using EducationPortal.DAL.DbContexts;
    using EducationPortal.DAL.Entities.EF;
    using Microsoft.EntityFrameworkCore;

    public class UserRepository : EFRepository<User>
    {
        public UserRepository(EFDbContext db)
            : base(db)
        {
        }

        protected override IQueryable<User> SelectQuery => this.db.Set<User>()
                                                   .Include(u => u.CompletedCourses)
                                                   .Include(u => u.CreatedCourses)
                                                   .Include(u => u.JoinedCourses)
                                                   .Include(u => u.LearnedMaterials)
                                                   .Include(u => u.UserSkills);

        public override User GetById(long id)
        {
            return this.SelectQuery.SingleOrDefault(u => u.Id == id);
        }
    }
}
