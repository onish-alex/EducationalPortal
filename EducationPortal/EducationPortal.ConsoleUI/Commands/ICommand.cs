using System;
using System.Collections.Generic;
using System.Text;

namespace EducationPortal.ConsoleUI.Commands
{
    public interface ICommand
    {
        string[] Result { get; }

        void Execute();
    }
}
