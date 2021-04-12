namespace EducationPortal.ConsoleUI
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using EducationPortal.ConsoleUI.Commands;
    using EducationPortal.ConsoleUI.Resources;

    public class CommandManager
    {
        private string[] commandParts;
        private bool exitFlag;
        private string exitCommandStr = "exit";
        private string helpCommandStr = "help";

        private ClientData clientData;
        private IDictionary<string, ICommand> commands;

        public CommandManager(ClientData clientData, IEnumerable<ICommand> commands)
        {
            this.commands = new Dictionary<string, ICommand>();

            foreach (var item in commands)
            {
                this.commands.Add(item.Name, item);
            }

            this.clientData = clientData;
            this.clientData.ConsoleStatePrefix = ConsoleMessages.DefaultCommandPrefix;
        }

        public void Run()
        {
            this.exitFlag = false;
            while (!this.exitFlag)
            {
                Console.Write("\n{0}> ", this.clientData.ConsoleStatePrefix);
                var inputStr = Console.ReadLine();

                if (string.IsNullOrWhiteSpace(inputStr))
                {
                    continue;
                }

                this.commandParts = inputStr
                                      .Trim()
                                      .Split(' ', StringSplitOptions.RemoveEmptyEntries);

                var commandStr = this.commandParts.First();
                this.clientData.InputBuffer.Clear();
                this.clientData.InputBuffer.AddRange(this.commandParts.Skip(1));

                if (commandStr == this.exitCommandStr)
                {
                    this.exitFlag = true;
                }
                else if (commandStr == this.helpCommandStr)
                {
                    Console.WriteLine(string.Join('\n', this.commands.Select(command => command.Value.Description)));
                }
                else if (this.commands.ContainsKey(commandStr))
                {
                    if (this.commands[commandStr].ParamsCount == this.clientData.InputBuffer.Count)
                    {
                        this.commands[commandStr].Execute();
                    }
                    else
                    {
                        Console.WriteLine(ConsoleMessages.ErrorWrongParamsCount);
                    }
                }
                else
                {
                    Console.WriteLine(string.Format(ConsoleMessages.ErrorUnknownCommand, commandStr));
                }
            }
        }
    }
}
