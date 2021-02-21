namespace EducationPortal.DAL.Repository.EF
{
    using EducationPortal.DAL.DbContexts;
    using EducationPortal.DAL.Entities.EF;

    public class AccountRepository : EFRepository<Account>
    {
        public AccountRepository(EFDbContext db)
            : base(db)
        {
        }
    }
}
