namespace EducationPortal.DAL.Repository.EF
{
    using System.Linq;
    using EducationPortal.DAL.DbContexts;
    using EducationPortal.DAL.Entities.EF;

    public class AccountRepository : EFRepository<Account>
    {
        public AccountRepository(EFDbContext db)
            : base(db)
        {
        }

        protected override IQueryable<Account> SelectQuery => this.db.Set<Account>();

        public override Account GetById(long id)
        {
            return this.SelectQuery.SingleOrDefault(a => a.Id == id);
        }
    }
}
