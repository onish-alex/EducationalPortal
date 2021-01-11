using System;
using EducationPortal.ConsoleUI.Commands;
using EducationPortal.ConsoleUI.Controllers;

namespace EducationPortal.ConsoleUI
{
    public class Program
    {
        //UserController userController;

        public static void Main(string[] args)
        {
            //UserController = new UserController();
            //RunCommandHandler();
        }

        public static void RunCommandHandler()
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

                var commandParts = inputStr
                                    .Trim()
                                    .Split(' ', StringSplitOptions.RemoveEmptyEntries);
                var command = commandParts[0];

                switch (command)
                {
                    case "reg":
                        break;

                    case "login":
                        break;

                    case "logout":
                        break;

                    case "exit":
                        exitFlag = true;
                        break;

                    default:
                        Console.WriteLine("\n\nError! Unrecognized command: \"{0}\". Please type \"help\" to see list of available commands\n\n", command);
                        break;
                }
            }
        }
    }
}
