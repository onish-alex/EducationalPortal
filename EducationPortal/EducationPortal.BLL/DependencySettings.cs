using Microsoft.Extensions.DependencyInjection;
using EducationPortal.DAL.Repository;
using EducationPortal.DAL.DbContexts;

namespace EducationPortal.BLL
{
    public static class DependencySettings
    {
        public static IServiceCollection GetDICollection()
        {
            var service = new ServiceCollection();
            service.AddSingleton(typeof(IRepository<>), typeof(FileRepository<>));
            service.AddSingleton<FileDbContext>();
            return service;
        }
    }
}
