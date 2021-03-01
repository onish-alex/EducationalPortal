namespace EducationPortal.DAL.Repository.EF
{
    using EducationPortal.DAL.DbContexts;
    using EducationPortal.DAL.Entities.EF;

    public class EFAccountRepository : EFRepository<Account>
    {
        public EFAccountRepository(EFDbContext db)
            : base(db)
        {
        }
    }
}
