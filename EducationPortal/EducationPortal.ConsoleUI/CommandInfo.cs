using System;
using System.Collections.Generic;

namespace EducationPortal.ConsoleUI
{
    public class CommandInfo
    {
        public int ParamsCount { get; set; }
        
        public string Description { get; set; }

        public Action Handler { get; set; }

        public CommandInfo()
        {

        }
    }
}
