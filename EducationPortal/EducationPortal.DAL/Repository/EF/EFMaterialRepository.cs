namespace EducationPortal.DAL.Repository.EF
{
    using EducationPortal.DAL.DbContexts;
    using EducationPortal.DAL.Entities.EF;

    public class EFMaterialRepository : EFRepository<Material>
    {
        public EFMaterialRepository(EFDbContext db)
            : base(db)
        {
        }
    }
}
