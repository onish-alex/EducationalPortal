namespace EducationPortal.DAL.Repository.EF
{
    using System.Linq;
    using EducationPortal.DAL.DbContexts;
    using EducationPortal.DAL.Entities.EF;
    using Microsoft.EntityFrameworkCore;

    public class MaterialRepository : EFRepository<Material>
    {
        public MaterialRepository(EFDbContext db)
            : base(db)
        {
        }

        protected override IQueryable<Material> SelectQuery => this.db.Set<Material>();

        public override Material GetById(long id)
        {
            return this.SelectQuery.SingleOrDefault(m => m.Id == id);
        }
    }
}
