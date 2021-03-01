namespace EducationPortal.BLL.Settings
{
    using EducationPortal.BLL.Mappers;
    using EducationPortal.DAL.DbContexts;
    using EducationPortal.DAL.Entities.EF;
    using EducationPortal.DAL.Repository.Base;
    using EducationPortal.DAL.Repository.EF;
    using Microsoft.Extensions.DependencyInjection;

    public static class DependencySettings
    {
        public static void AddBLLDependencies(this IServiceCollection services)
        {
            services.AddDbContext<EFDbContext>();
            services.AddSingleton<IRepository<User>, EFUserRepository>();
            services.AddSingleton<IRepository<Material>, EFMaterialRepository>();
            services.AddSingleton<IRepository<Course>, EFCourseRepository>();
            services.AddSingleton<IRepository<Skill>, EFSkillRepository>();
            services.AddSingleton<IRepository<Account>, EFAccountRepository>();
            services.AddSingleton<IMapper, EntityMapper>();
        }
    }
}
