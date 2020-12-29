using System;
using System.Collections.Generic;
using EducationalPortal.DAL.Entities;
using EducationalPortal.DAL.Utilities;

namespace EducationalPortal.DAL.DbContexts
{
    public class FileDbContext
    {
        public IDictionary<DbConfig.TableNames, DbTable<Entity>> _tables;

        public FileDbContext()
        {
            _tables = new Dictionary<DbConfig.TableNames, DbTable<Entity>>();
        }

        public DbTable<Entity> GetTable<T>() where T : Entity
        {
            var tableNameStr = typeof(T).Name;
            var tableName = (DbConfig.TableNames)Enum.Parse(typeof(DbConfig.TableNames), tableNameStr);
            if (!_tables.ContainsKey(tableName))
            {
                var fileName = DbConfig.dbPathPrefix + DbConfig.TablePaths[tableName];
                IEnumerable<T> tableContent = JsonFileHelper.JsonRead<T>(fileName);
                var table = new DbTable<Entity>(tableContent);
                _tables.Add(tableName, table);
            }
            return _tables[tableName];
        }

        public void Save()
        {
            foreach (var key in _tables.Keys)
            {
                if (!_tables[key].IsSynch)
                {
                    var fileName = DbConfig.dbPathPrefix + DbConfig.TablePaths[key];
                    JsonFileHelper.JsonWrite(fileName, _tables[key].Content);
                    _tables[key].IsSynch = true;
                }
            }
        }
    }
}
