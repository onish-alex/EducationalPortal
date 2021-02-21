namespace EducationPortal.DAL.Repository.EF
{
    using EducationPortal.DAL.DbContexts;
    using EducationPortal.DAL.Entities.EF;

    public class MaterialRepository : EFRepository<Material>
    {
        public MaterialRepository(EFDbContext db)
            : base(db)
        {
        }
    }
}
