namespace EducationPortal.ConsoleUI
{
    using System;

    public class CommandInfo
    {
        public CommandInfo()
        {
        }

        public int ParamsCount { get; set; }

        public string Description { get; set; }

        public Action Handler { get; set; }
    }
}
