using System;
using System.Collections.Generic;
using EducationPortal.ConsoleUI.Commands;
using EducationPortal.BLL.DTO;
using EducationPortal.BLL.Services;
using System.Linq;

namespace EducationPortal.ConsoleUI
{
    public class CommandManager
    {
        private IDictionary<string, IService> services;
        private IEnumerable<string> output;
        private IDictionary<string, Action> commandHandlers;
        private IEnumerable<string> commandParts;
        private DTOBuilder builder;

        public CommandManager(IEnumerable<IService> services)
        {
            this.builder = DTOBuilder.GetInstance();
            this.services = new Dictionary<string, IService>();

            foreach (var item in services)
            {
                this.services.Add(item.Name, item);
            }

            commandHandlers = new Dictionary<string, Action>();
            commandHandlers.Add("reg", () =>
            {
                var account = builder.GetAccount(commandParts.ToArray(), true);
                var user = builder.GetUser(commandParts.Skip(typeof(AccountDTO).GetProperties().Length).ToArray());
                var command = new RegisterCommand(this.services["User"] as IUserService, user, account);
                command.Execute();
                output = command.Result;
            });

            commandHandlers.Add("login", () =>
            {
                var command = new AuthorizeCommand(this.services["User"] as IUserService, builder.GetAccount(commandParts.ToArray()));
                command.Execute();
                output = command.Result;
            });
        }


        public void Run()
        {
            var exitFlag = false;
            while (!exitFlag)
            {
                Console.Write("\nEducationPortal> ");
                var inputStr = Console.ReadLine();

                if (string.IsNullOrWhiteSpace(inputStr))
                {
                    continue;
                }

                commandParts = inputStr
                                .Trim()
                                .Split(' ', StringSplitOptions.RemoveEmptyEntries);

                var commandStr = commandParts.First();
                commandParts = commandParts.Skip(1);

                if (commandHandlers.ContainsKey(commandStr))
                {
                    commandHandlers[commandStr].Invoke();
                } 
                else
                {
                    output = new string[] { string.Format("\n\nUnrecognized command: \"{0}\". Please type \"help\" to see list of available commands\n\n", commandStr) };
                }

                foreach (var item in output)
                {
                    Console.WriteLine(item);
                }
            }
        }
    }
}
