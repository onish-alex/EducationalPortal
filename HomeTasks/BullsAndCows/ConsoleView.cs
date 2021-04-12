using System;

namespace BullsAndCows
{
    public class ConsoleView
    {
        private readonly GameLogic _gameLogic;

        public ConsoleView(GameLogic gameLogic)
        {
            _gameLogic = gameLogic;
            RunCommandHandler();
        }

        public void RunCommandHandler()
        {
            bool isStopped = false;
            while (!isStopped)
            {
                Console.Write("\n>");
                var input = Console.ReadLine();
                if (string.IsNullOrWhiteSpace(input))
                    continue;
                
                var commandParts = input.Trim().Split(' ', StringSplitOptions.RemoveEmptyEntries);

                var command = commandParts[0];
                
                switch (command)
                {
                    case "start":
                        _gameLogic.InitGameData();
                        RunGameProcess();
                        break;

                    case "setcount":

                        int count = GameConfig.MinNumbersCount;

                        if (_gameLogic.IsInit)
                        {
                            Console.WriteLine("Error: Game is already running!");
                        }

                        if (commandParts.Length < 2 
                         || !int.TryParse(commandParts[1], out count)
                         || count <= GameConfig.MinNumbersCount 
                         || count >= GameConfig.MaxNumbersCount) 
                        {
                            Console.WriteLine("Wrong count of numbers!");
                        }

                        _gameLogic.NumbersCount = count;
                        
                        break;

                    case "help":
                        Console.WriteLine("\"start\" - start game");
                        Console.WriteLine("\"setcount d\" - set count of numbers in \"d\" value; d is integer");
                        Console.WriteLine("\"exit\"");
                        break;

                    case "exit":
                        isStopped = true;
                        break;

                    default:
                        Console.WriteLine("\n\nError! Unrecognized command: {0}. Please type \"help\" to see list of available commands\n\n", command);
                        break;
                }
            }
        }

        public void RunGameProcess()
        {
            while(_gameLogic.IsInit)
            {
                Console.WriteLine(_gameLogic.GetAnswer());
                if (!_gameLogic.IsInit)
                    continue;

                var parameters = Console.ReadLine().Trim().Split(' ', StringSplitOptions.RemoveEmptyEntries);
                
                if (parameters.Length != 2)
                {
                    Console.WriteLine("There's might be only two numbers, bull and cows. Please try again");
                    continue;
                }

                if (!int.TryParse(parameters[0], out int bulls)
                 || !int.TryParse(parameters[1], out int cows)
                 || bulls > _gameLogic.NumbersCount
                 || cows > _gameLogic.NumbersCount
                 || bulls < 0
                 || cows < 0)
                {
                    Console.WriteLine("One or more parameters inputed wrong. Please try again");
                    continue;
                }

                _gameLogic.SendParams(bulls, cows);
            }
        }
    }
}
