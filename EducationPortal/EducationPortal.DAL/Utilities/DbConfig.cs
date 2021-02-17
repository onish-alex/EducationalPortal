using System.Collections.Generic;

namespace EducationPortal.DAL.Utilities
{
    public static class DbConfig
    {
        public static string dbPathPrefix = "../../../../db/";
        public static string dbIdsFileName = "ids.txt";
        public static readonly IDictionary<TableNames, string> TablePaths = new Dictionary<TableNames, string>()
        {
            { TableNames.User, "users/" },
            { TableNames.Account, "accounts/" },
            { TableNames.Course, "courses/" },
            { TableNames.Material, "materials/" },
            { TableNames.Skill, "skills/" },
        };
    }
}
