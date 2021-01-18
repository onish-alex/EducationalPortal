using System;
using System.Linq;
using System.Collections.Generic;
using EducationPortal.ConsoleUI.Commands;
using EducationPortal.BLL.DTO;
using EducationPortal.BLL.Services;

namespace EducationPortal.ConsoleUI
{
    public class CommandManager
    {
        private IDictionary<string, IService> services;
        private IDictionary<string, Action> commandHandlers;
        private string[] commandParts;
        private List<string> output;
        private DTOBuilder dtoBuilder;
        private Client client;
        private string consoleStatePrefix;
        private bool exitFlag;

        public CommandManager(IEnumerable<IService> services)
        {
            this.dtoBuilder = DTOBuilder.GetInstance();
            this.consoleStatePrefix = ConsoleMessages.DefaultCommandPrefix;
            this.output = new List<string>();
            this.services = new Dictionary<string, IService>();

            foreach (var item in services)
            {
                this.services.Add(item.Name, item);
            }

            commandHandlers = new Dictionary<string, Action>();

            commandHandlers.Add("reg", () =>
            {
                if (client != null)
                {
                    output.Add(ConsoleMessages.ErrorTryRegWhileLoggedIn);
                    return;
                }

                var account = dtoBuilder.GetAccount(commandParts, true);
                var user = dtoBuilder.GetUser(commandParts.Skip(typeof(AccountDTO).GetProperties().Length).ToArray());
                var command = new RegisterCommand(this.services["User"] as IUserService, user, account);
                command.Execute();
                var response = command.Response;
                output.Add(response.Message);
            });

            commandHandlers.Add("login", () =>
            {
                if (client != null)
                {
                    this.output.Add(ConsoleMessages.ErrorTryLogInWhileLoggedIn);
                    return;
                }

                var account = dtoBuilder.GetAccount(commandParts.ToArray());
                var command = new AuthorizeCommand(this.services["User"] as IUserService, account);
                command.Execute();
                var response = command.Response;
                this.output.Add(response.Message);

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
                    consoleStatePrefix = ConsoleMessages.DefaultCommandPrefix;
                    output.Add(ConsoleMessages.InfoLoggedOut);
                }
                else
                {
                    output.Add(ConsoleMessages.ErrorTryLogOutWhileLoggedOut);
                }

            });

            commandHandlers.Add("help", () =>
            {
                output.AddRange(CommandInfo.Storage.Select(command => command.Value.Description));
            });

            commandHandlers.Add("exit", () =>
            {
                exitFlag = true;
            });
        }

        public void Run()
        {
            exitFlag = false;
            while (!exitFlag)
            {
                foreach (var item in output)
                {
                    Console.WriteLine(item);
                }

                output.Clear();

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
                commandParts = commandParts.Skip(1).ToArray();

                if (CommandInfo.Storage.ContainsKey(commandStr))
                {
                    if (CommandInfo.Storage[commandStr].ParamsCount == commandParts.Length)
                    {
                        commandHandlers[commandStr].Invoke();
                    }
                    else
                    {
                        output.Add(ConsoleMessages.ErrorWrongParamsCount);
                    }
                } 
                else
                {
                    output.Add(string.Format(ConsoleMessages.ErrorUnknownCommand, commandStr));
                }
            }
        }
    }
}
