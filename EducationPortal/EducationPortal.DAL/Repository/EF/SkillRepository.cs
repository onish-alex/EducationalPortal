namespace EducationPortal.DAL.Repository.EF
{
    using System.Linq;
    using EducationPortal.DAL.DbContexts;
    using EducationPortal.DAL.Entities.EF;

    public class SkillRepository : EFRepository<Skill>
    {
        public SkillRepository(EFDbContext db)
            : base(db)
        {
        }

        protected override IQueryable<Skill> SelectQuery => this.db.Set<Skill>();

        public override Skill GetById(long id)
        {
            return this.SelectQuery.SingleOrDefault(s => s.Id == id);
        }
    }
}
