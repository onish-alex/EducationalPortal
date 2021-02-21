namespace EducationPortal.DAL.Repository.EF
{
    using EducationPortal.DAL.DbContexts;
    using EducationPortal.DAL.Entities.EF;

    public class SkillRepository : EFRepository<Skill>
    {
        public SkillRepository(EFDbContext db)
            : base(db)
        {
        }
    }
}
