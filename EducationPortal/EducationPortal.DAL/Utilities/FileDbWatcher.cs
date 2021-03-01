namespace EducationPortal.DAL.Utilities
{
    using System;
    using System.Collections.Generic;
    using System.IO;

    public class FileDbWatcher
    {
        private static readonly FileDbWatcher Instance = new FileDbWatcher();
        private Dictionary<TableNames, FileSystemWatcher> watchers;

        private FileDbWatcher()
        {
            this.watchers = new Dictionary<TableNames, FileSystemWatcher>();
            var tableNames = Enum.GetNames(typeof(TableNames));
            foreach (var name in tableNames)
            {
                var tableEnumValue = (TableNames)Enum.Parse(typeof(TableNames), name);
                var tablePath = FileDbConfig.TablePaths[tableEnumValue];
                this.watchers.Add(tableEnumValue, new FileSystemWatcher(FileDbConfig.DbPathPrefix + tablePath));
                this.watchers[tableEnumValue].EnableRaisingEvents = true;
            }
        }

        public FileSystemWatcher this[TableNames tableName]
        {
            get
            {
                return this.watchers[tableName];
            }
        }

        public static FileDbWatcher GetInstance()
        {
            return Instance;
        }
    }
}
