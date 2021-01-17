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
        private IDictionary<string, Action> commandHandlers;
        private IEnumerable<string> commandParts;
        private IEnumerable<string> output;
        private DTOBuilder dtoBuilder;
        private Client client;
        private string consoleStatePrefix;
        private bool exitFlag;


        public CommandManager(IEnumerable<IService> services)
        {
            this.dtoBuilder = DTOBuilder.GetInstance();
            this.consoleStatePrefix = "EducationPortal";

            this.services = new Dictionary<string, IService>();

            foreach (var item in services)
            {
                this.services.Add(item.Name, item);
            }

            commandHandlers = new Dictionary<string, Action>();
            
            commandHandlers.Add("reg", () =>
            {
                var account = dtoBuilder.GetAccount(commandParts.ToArray(), true);
                var user = dtoBuilder.GetUser(commandParts.Skip(typeof(AccountDTO).GetProperties().Length).ToArray());
                var command = new RegisterCommand(this.services["User"] as IUserService, user, account);
                command.Execute();
                var response = command.Response;
                output = new string[] { response.Message };
            });

            commandHandlers.Add("login", () =>
            {
                if (client != null)
                {
                    output = new string[] { "You are already logged in!" };
                    return;
                }

                var account = dtoBuilder.GetAccount(commandParts.ToArray());
                var command = new AuthorizeCommand(this.services["User"] as IUserService, account);
                command.Execute();
                var response = command.Response;
                output = new string[] { response.Message };

                if (response.IsSuccessful)
                {
                    this.client = Client.GetInstance();
                    this.client.Id = response.Id;
                    this.client.Info = response.User;
                    this.consoleStatePrefix = client.Info.Name;
                }
            });

            commandHandlers.Add("logout", () =>
            {
                if (this.client != null)
                {
                    this.client = null;
                    consoleStatePrefix = "EducationPortal";
                    output = new string[] { "Successfully logged out!" };
                }
                else
                {
                    output = new string[] { "You're not logged in!" };
                }
                
            });

            commandHandlers.Add("exit", () =>
            {
                exitFlag = true;
            });
        }

        public void Run()
        {
            exitFlag = false;
            output = Enumerable.Empty<string>();
            while (!exitFlag)
            {
                foreach (var item in output)
                {
                    Console.WriteLine(item);
                }

                Console.Write("\n{0}> ", consoleStatePrefix);
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

                
            }
        }
    }
}
