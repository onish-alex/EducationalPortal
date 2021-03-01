namespace EducationPortal.DAL.Repository.EF
{
    using EducationPortal.DAL.DbContexts;
    using EducationPortal.DAL.Entities.EF;

    public class EFSkillRepository : EFRepository<Skill>
    {
        public EFSkillRepository(EFDbContext db)
            : base(db)
        {
        }
    }
}
