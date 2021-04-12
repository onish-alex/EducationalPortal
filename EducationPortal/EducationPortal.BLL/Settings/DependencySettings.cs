namespace EducationPortal.BLL.Settings
{
    using System.Configuration;
    using EducationPortal.BLL.DTO;
    using EducationPortal.BLL.Mappers;
    using EducationPortal.BLL.Services;
    using EducationPortal.BLL.Validation;
    using EducationPortal.DAL.DbContexts;
    using EducationPortal.DAL.Entities.EF;
    using EducationPortal.DAL.Repository.Base;
    using EducationPortal.DAL.Repository.EF;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;

    public static class DependencySettings
    {
        public static void AddBLLDependencies(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<EFDbContext>(options =>
                options.UseSqlServer(
                    configuration.GetConnectionString("EducationPortalDB")));

            AddRepositories(services);
            services.AddScoped<IMapper, EntityMapper>();
        }

        public static void AddBLLDependencies(this IServiceCollection services)
        {
            services.AddDbContext<EFDbContext>(options =>
                options.UseSqlServer(
                    ConfigurationManager.ConnectionStrings["EducationPortalDB"].ConnectionString));

            AddRepositories(services);
            services.AddScoped<IMapper, EntityMapper>();
        }

        public static void AddRepositories(this IServiceCollection services)
        {
            services.AddScoped(typeof(IRepository<>), typeof(EFRepository<>));
            services.AddScoped<IRepository<User>, EFUserRepository>();
        }
    }
}
