namespace EducationPortal.ConsoleUI
{
    using EducationPortal.BLL.DTO;
    using EducationPortal.BLL.Services;
    using EducationPortal.BLL.Settings;
    using EducationPortal.BLL.Validation;
    using EducationPortal.ConsoleUI.Commands;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.DependencyInjection.Extensions;

    public class Program
    {
        public static void Main(string[] args)
        {
            var service = new ServiceCollection();

            service.AddBLLDependencies();

            service.AddSingleton<IUserService, UserService>();
            service.AddSingleton<ICourseService, CourseService>();
            service.AddSingleton<IMaterialService, MaterialService>();

            service.AddSingleton<IValidator<UserDTO>, UserValidator>(provider => new UserValidator());
            service.AddSingleton<IValidator<MaterialDTO>, MaterialValidator>();
            service.AddSingleton<IValidator<AccountDTO>, AccountValidator>(provider => new AccountValidator());
            service.AddSingleton<IValidator<SkillDTO>, SkillValidator>();
            service.AddSingleton<IValidator<CourseDTO>, CourseValidator>();

            service.AddSingleton<ClientData>();
            service.AddCommands();

            var provider = service.BuildServiceProvider();

            var handler = new CommandManager(provider.GetService<ClientData>(), provider.GetServices<ICommand>());
            handler.Run();
        }
    }
}
