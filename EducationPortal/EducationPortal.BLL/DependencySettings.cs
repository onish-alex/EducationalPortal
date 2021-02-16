namespace EducationPortal.BLL
{
    using EducationPortal.DAL.DbContexts;
    using EducationPortal.DAL.Entities.EF;
    using EducationPortal.DAL.Repository.Base;
    using EducationPortal.DAL.Repository.EF;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.DependencyInjection;

    public static class DependencySettings
    {
        public static IServiceCollection GetDICollection()
        {
            var service = new ServiceCollection();
            service.AddDbContext<EFDbContext>();
            service.AddSingleton<IRepository<User>, UserRepository>();
            service.AddSingleton<IRepository<Material>, MaterialRepository>();
            service.AddSingleton<IRepository<Course>, CourseRepository>();
            service.AddSingleton<IRepository<Skill>, SkillRepository>();
            service.AddSingleton<IRepository<Account>, AccountRepository>();
            return service;
        }
    }
}
