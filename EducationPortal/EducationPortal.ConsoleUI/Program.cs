namespace EducationPortal.ConsoleUI
{
    using EducationPortal.BLL;
    using EducationPortal.BLL.Services;
    using EducationPortal.ConsoleUI.Validation;
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

            service.TryAddEnumerable(new[]
            {
                ServiceDescriptor.Singleton<IValidator, CourseDataValidator>(),
                ServiceDescriptor.Singleton<IValidator, MaterialDataValidator>(),
                ServiceDescriptor.Singleton<IValidator, RegisterDataValidator>(),
                ServiceDescriptor.Singleton<IValidator, SkillDataValidator>(),
            });

            var provider = service.BuildServiceProvider();

            var handler = new CommandManager(provider.GetServices<IService>(), provider.GetServices<IValidator>());
            handler.Run();
        }
    }
}
