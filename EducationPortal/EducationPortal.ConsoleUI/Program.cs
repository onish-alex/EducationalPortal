using EducationPortal.BLL.Services;
using EducationPortal.BLL;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace EducationPortal.ConsoleUI
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var service = new ServiceCollection();
            service.Add(DependencySettings.GetDICollection());
            service.TryAddEnumerable(new[]
                {
                    ServiceDescriptor.Singleton<IService, UserService>(),
                    ServiceDescriptor.Singleton<IService, CourseService>(),
                    ServiceDescriptor.Singleton<IService, MaterialService>(),
                });

            var provider = service.BuildServiceProvider();

            var handler = new CommandManager(provider.GetServices<IService>());
            handler.Run();
        }
    }
}
