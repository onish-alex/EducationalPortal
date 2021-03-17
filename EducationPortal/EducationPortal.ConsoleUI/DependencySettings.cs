namespace EducationPortal.ConsoleUI
{
    using EducationPortal.ConsoleUI.Commands;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.DependencyInjection.Extensions;

    public static class DependencySettings
    {
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
