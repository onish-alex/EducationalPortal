using System.Collections.Generic;

namespace EducationalPortal.DAL.Utilities
{
    public static class DbConfig
    {
        public enum TableNames
        {
            User,
            Account,
            Role,
        };

        public static string dbPathPrefix = "../../../db/";
        public static readonly IDictionary<TableNames, string> TablePaths = new Dictionary<TableNames, string>()
        {
            { TableNames.User, "users.json" },
            { TableNames.Account, "accounts.json" },
            { TableNames.Role, "roles.json" },
        };
    }
}
