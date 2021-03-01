namespace EducationPortal.ConsoleUI
{
    using EducationPortal.BLL.Settings;
    using EducationPortal.ConsoleUI.Commands;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.DependencyInjection.Extensions;

    public class Program
    {
        public static void Main(string[] args)
        {
            var service = new ServiceCollection();

            service.AddBLLDependencies();
            service.AddBusinessServices();
            service.AddValidators();
            service.AddSingleton<ClientData>();
            service.AddCommands();

            var provider = service.BuildServiceProvider();

            var handler = new CommandManager(provider.GetService<ClientData>(), provider.GetServices<ICommand>());
            handler.Run();
        }
    }
}
