using System;
using System.Collections.Generic;
using System.IO;

namespace EducationPortal.DAL.Utilities
{
    public class DbWatcher
    {
        private static readonly DbWatcher instance = new DbWatcher();
        private Dictionary<TableNames, FileSystemWatcher> watchers;

        private DbWatcher()
        {
            watchers = new Dictionary<TableNames, FileSystemWatcher>();
            var tableNames = Enum.GetNames(typeof(TableNames));
            foreach (var name in tableNames)
            {
                var tableEnumValue = (TableNames)Enum.Parse(typeof(TableNames), name);
                var tablePath = DbConfig.TablePaths[tableEnumValue];
                watchers.Add(tableEnumValue, new FileSystemWatcher(DbConfig.dbPathPrefix + tablePath));
                watchers[tableEnumValue].EnableRaisingEvents = true;
            }
        }

        public static DbWatcher GetInstance()
        {
            return instance;
        }

        public FileSystemWatcher this[TableNames tableName] 
        {
            get
            {
                return watchers[tableName];
            }
        }
    }
}
