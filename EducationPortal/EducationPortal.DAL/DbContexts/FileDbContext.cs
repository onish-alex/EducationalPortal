using System;
using System.IO;
using System.Collections.Generic;
using EducationPortal.DAL.Entities;
using EducationPortal.DAL.Utilities;

namespace EducationPortal.DAL.DbContexts
{
    public class FileDbContext
    {
        private IDictionary<TableNames, DbTable> tables;
        private JsonFileHelper fileHelper;
        private DbWatcher fileWatcher;

        public FileDbContext()
        {
            tables = new Dictionary<TableNames, DbTable>();
            fileHelper = JsonFileHelper.GetInstance();
            fileWatcher = DbWatcher.GetInstance();

            fileWatcher[TableNames.User].Created += OnCreate<User>;
            fileWatcher[TableNames.User].Changed += OnChange<User>;
            fileWatcher[TableNames.User].Deleted += OnDelete<User>;
            fileWatcher[TableNames.Account].Created += OnCreate<Account>;
            fileWatcher[TableNames.Account].Changed += OnChange<Account>;
            fileWatcher[TableNames.Account].Deleted += OnDelete<Account>;
        }

        public IDbTable GetTable<T>() where T : Entity
        {
            var tableNameStr = typeof(T).Name;
            var tableName = (TableNames)Enum.Parse(typeof(TableNames), tableNameStr);
            
            if (!tables.ContainsKey(tableName))
            {
                var tablePath = DbConfig.dbPathPrefix + DbConfig.TablePaths[tableName];
                IEnumerable<T> tableContent = fileHelper.ReadTable<T>(tablePath);
                var table = new DbTable(tableContent);
                tables.Add(tableName, table);
            }
            
            return tables[tableName];
        }

        public void Save<T>()
        {
            var tableNameStr = typeof(T).Name;
            var tableName = (TableNames)Enum.Parse(typeof(TableNames), tableNameStr);

            var entitiesToSave = new Dictionary<Entity, EntityState>();

            var currentTable = tables[tableName];
            var tablePath = DbConfig.dbPathPrefix + DbConfig.TablePaths[tableName];

            foreach (var entity in currentTable.ContentWithState)
            {
                if (entity.Value != EntityState.Synchronized)
                {
                    entitiesToSave.Add(entity.Key, entity.Value);
                }
            }

            fileWatcher[tableName].EnableRaisingEvents = false;
            fileHelper.SaveTable(tablePath, entitiesToSave);
            fileWatcher[tableName].EnableRaisingEvents = true;

            foreach (var entity in entitiesToSave.Keys)
            {
                if (currentTable.GetEntityState(entity) == EntityState.Deleted)
                {
                    currentTable.RemoveFromTable(entity);
                }
                else
                {
                    currentTable.SetEntityState(entity, EntityState.Synchronized);
                }
            }
        }

        private void OnCreate<T>(object sender, FileSystemEventArgs args) where T: Entity
        {
            var tablePath = Path.GetDirectoryName(args.FullPath) + "/";
            var idStr = Path.GetFileNameWithoutExtension(args.FullPath);
            var isParsed = long.TryParse(idStr, out long id);
            if (isParsed)
            {
                var createdEntity = fileHelper.ReadEntity<T>(tablePath, id);
                var tableNameStr = typeof(T).Name;
                var tableName = (TableNames)Enum.Parse(typeof(TableNames), tableNameStr);

                if (!tables.ContainsKey(tableName))
                {
                    IEnumerable<T> tableContent = fileHelper.ReadTable<T>(tablePath);
                    var table = new DbTable(tableContent);
                    tables.Add(tableName, table);
                }

                tables[tableName].Add(createdEntity);
                tables[tableName].SetEntityState(createdEntity, EntityState.Synchronized);
            }
        }

        private void OnChange<T>(object sender, FileSystemEventArgs args) where T : Entity
        {
            var tablePath = Path.GetDirectoryName(args.FullPath) + "/";
            var idStr = Path.GetFileNameWithoutExtension(args.FullPath);
            var isParsed = long.TryParse(idStr, out long id);
            if (isParsed)
            {
                var changedEntity = fileHelper.ReadEntity<T>(tablePath, id);
                var tableNameStr = typeof(T).Name;
                var tableName = (TableNames)Enum.Parse(typeof(TableNames), tableNameStr);

                if (!tables.ContainsKey(tableName))
                {
                    IEnumerable<T> tableContent = fileHelper.ReadTable<T>(tablePath);
                    var table = new DbTable(tableContent);
                    tables.Add(tableName, table);
                }

                tables[tableName].Update(changedEntity);
                tables[tableName].SetEntityState(changedEntity, EntityState.Synchronized);
            }
        }

        private void OnDelete<T>(object sender, FileSystemEventArgs args) where T : Entity, new()
        {
            var tablePath = Path.GetDirectoryName(args.FullPath) + "/";
            var idStr = Path.GetFileNameWithoutExtension(args.FullPath);
            var isParsed = long.TryParse(idStr, out long id);
            
            if (isParsed)
            {
                var tableNameStr = typeof(T).Name;
                var tableName = (TableNames)Enum.Parse(typeof(TableNames), tableNameStr);

                if (!tables.ContainsKey(tableName))
                {
                    IEnumerable<T> tableContent = fileHelper.ReadTable<T>(tablePath);
                    var table = new DbTable(tableContent);
                    tables.Add(tableName, table);
                }

                tables[tableName].Remove(new T() { Id = id });
            }
        }
    }
}
