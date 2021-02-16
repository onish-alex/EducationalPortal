namespace EducationPortal.ConsoleUI.Commands
{
    using EducationPortal.BLL.Response;

    public interface ICommand<T>
        where T : IResponse
    {
        T Response { get; }

        void Execute();
    }
}
