using EducationPortal.BLL.Response;

namespace EducationPortal.ConsoleUI.Commands
{
    public interface ICommand<T> where T : IResponse
    {
        T Response { get; }

        void Execute();
    }
}
