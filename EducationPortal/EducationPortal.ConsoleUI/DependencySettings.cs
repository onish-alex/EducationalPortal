namespace EducationPortal.ConsoleUI
{
    using EducationPortal.BLL.DTO;
    using EducationPortal.BLL.Services;
    using EducationPortal.BLL.Validation;
    using EducationPortal.ConsoleUI.Commands;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.DependencyInjection.Extensions;

    public static class DependencySettings
    {
        public static void AddBusinessServices(this ServiceCollection service)
        {
            service.AddSingleton<IUserService, UserService>();
            service.AddSingleton<ICourseService, CourseService>();
            service.AddSingleton<IMaterialService, MaterialService>();
        }

        public static void AddValidators(this ServiceCollection service)
        {
            service.AddSingleton<IValidator<UserDTO>, UserValidator>();
            service.AddSingleton<IValidator<MaterialDTO>, MaterialValidator>();
            service.AddSingleton<IValidator<AccountDTO>, AccountValidator>();
            service.AddSingleton<IValidator<SkillDTO>, SkillValidator>();
            service.AddSingleton<IValidator<CourseDTO>, CourseValidator>();
        }

        public static void AddCommands(this ServiceCollection service)
        {
            service.TryAddEnumerable(new[]
            {
                ServiceDescriptor.Singleton<ICommand, RegisterCommand>(),
                ServiceDescriptor.Singleton<ICommand, AuthorizeCommand>(),
                ServiceDescriptor.Singleton<ICommand, DeauthorizeCommand>(),
                ServiceDescriptor.Singleton<ICommand, AddCourseCommand>(),
                ServiceDescriptor.Singleton<ICommand, GetUserCoursesCommand>(),
                ServiceDescriptor.Singleton<ICommand, GetAllCoursesCommand>(),
                ServiceDescriptor.Singleton<ICommand, EnterCourseCommand>(),
                ServiceDescriptor.Singleton<ICommand, EditCourseCommand>(),
                ServiceDescriptor.Singleton<ICommand, AddSkillCommand>(),
                ServiceDescriptor.Singleton<ICommand, RemoveSkillCommand>(),
                ServiceDescriptor.Singleton<ICommand, GetCourseInfoCommand>(),
                ServiceDescriptor.Singleton<ICommand, AddMaterialCommand>(),
                ServiceDescriptor.Singleton<ICommand, LeaveCourseCommand>(),
                ServiceDescriptor.Singleton<ICommand, JoinCourseCommand>(),
                ServiceDescriptor.Singleton<ICommand, GetUserInfoCommand>(),
                ServiceDescriptor.Singleton<ICommand, GetNextMaterialCommand>(),
                ServiceDescriptor.Singleton<ICommand, FinishCourseCommand>(),
                ServiceDescriptor.Singleton<ICommand, GetJoinedCoursesCommand>(),
                ServiceDescriptor.Singleton<ICommand, GetCompletedCoursesCommand>(),
            });
        }
    }
}
