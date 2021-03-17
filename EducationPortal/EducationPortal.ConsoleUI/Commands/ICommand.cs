namespace EducationPortal.ConsoleUI.Commands
{
    public interface ICommand
    {
        string Name { get; }

        string Description { get; }

        int ParamsCount { get; }

        void Execute();
    }
}
