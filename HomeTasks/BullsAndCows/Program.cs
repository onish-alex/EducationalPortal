using System;
using System.Collections.Generic;

namespace BullsAndCows
{
    class Program
    {
        static void Main(string[] args)
        {
            var view = new ConsoleView(new GameLogic());
        }
    }
}
