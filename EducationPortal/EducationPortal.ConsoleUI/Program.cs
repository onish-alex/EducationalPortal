namespace EducationPortal.ConsoleUI
{
    using EducationPortal.BLL;
    using EducationPortal.BLL.Services;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.DependencyInjection.Extensions;

    public class Program
    {
        public static void Main(string[] args)
        {
            var service = new ServiceCollection
            {
                DependencySettings.GetDICollection(),
            };

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
