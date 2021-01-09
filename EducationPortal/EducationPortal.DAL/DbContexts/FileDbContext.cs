using System;
using System.IO;
using System.Collections.Generic;
using EducationPortal.DAL.Entities;
using EducationPortal.DAL.Utilities;

namespace EducationPortal.DAL.DbContexts
{
    public class FileDbContext
    {
        private IDictionary<TableNames, DbTable<Entity>> _tables;
        private JsonFileHelper fileHelper;
        private DbWatcher fileWatcher;

        public FileDbContext()
        {
            _tables = new Dictionary<TableNames, DbTable<Entity>>();
            fileHelper = JsonFileHelper.GetInstance();
            fileWatcher = DbWatcher.GetInstance();

            fileWatcher[TableNames.User].Created += OnCreate<User>;
            fileWatcher[TableNames.User].Changed += OnChange<User>;
            fileWatcher[TableNames.User].Deleted += OnDelete<User>;
            fileWatcher[TableNames.Account].Created += OnCreate<Account>;
            fileWatcher[TableNames.Account].Changed += OnChange<Account>;
            fileWatcher[TableNames.Account].Deleted += OnDelete<Account>;

        }

        public IDbTable<Entity> GetTable<T>() where T : Entity
        {
            var tableNameStr = typeof(T).Name;
            var tableName = (TableNames)Enum.Parse(typeof(TableNames), tableNameStr);
            if (!_tables.ContainsKey(tableName))
            {
                var tablePath = DbConfig.dbPathPrefix + DbConfig.TablePaths[tableName];
                IEnumerable<T> tableContent = fileHelper.ReadTable<T>(tablePath);
                var table = new DbTable<Entity>(tableContent);
                _tables.Add(tableName, table);
            }
            return _tables[tableName];
        }

        public void Save()
        {
            foreach (var table in _tables)
            {
                var tablePath = DbConfig.dbPathPrefix + DbConfig.TablePaths[table.Key];
                var entitiesToSave = new Dictionary<Entity, EntityState>();
                foreach (var entity in table.Value.ContentWithState)
                {
                    if (entity.Value != EntityState.Synchronized)
                    {
                        entitiesToSave.Add(entity.Key, entity.Value);
                    }
                }

                fileWatcher[table.Key].EnableRaisingEvents = false;
                fileHelper.SaveTable(tablePath, entitiesToSave);
                fileWatcher[table.Key].EnableRaisingEvents = true;

                foreach (var entity in entitiesToSave.Keys)
                {
                    if (table.Value.GetEntityState(entity) == EntityState.Deleted)
                    {
                        table.Value.RemoveFromTable(entity);
                    }
                    else
                    {
                        table.Value.SetEntityState(entity, EntityState.Synchronized);
                    }
                }
            }
        }

        private void OnCreate<T>(object sender, FileSystemEventArgs args) where T: Entity
        {
            var tablePath = Path.GetDirectoryName(args.FullPath) + "/";
            //+ "/"
            var idStr = Path.GetFileNameWithoutExtension(args.FullPath);
            var isParsed = long.TryParse(idStr, out long id);
            if (isParsed)
            {
                var createdEntity = fileHelper.ReadEntity<T>(tablePath, id);
                var tableNameStr = typeof(T).Name;
                var tableName = (TableNames)Enum.Parse(typeof(TableNames), tableNameStr);

                if (!_tables.ContainsKey(tableName))
                {
                    //var tablePath = DbConfig.dbPathPrefix + DbConfig.TablePaths[tableName];
                    IEnumerable<T> tableContent = fileHelper.ReadTable<T>(tablePath);
                    var table = new DbTable<Entity>(tableContent);
                    _tables.Add(tableName, table);
                }

                _tables[tableName].Add(createdEntity);
                _tables[tableName].SetEntityState(createdEntity, EntityState.Synchronized);
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

                if (!_tables.ContainsKey(tableName))
                {
                    //var tablePath = DbConfig.dbPathPrefix + DbConfig.TablePaths[tableName];
                    IEnumerable<T> tableContent = fileHelper.ReadTable<T>(tablePath);
                    var table = new DbTable<Entity>(tableContent);
                    _tables.Add(tableName, table);
                }

                _tables[tableName].Update(changedEntity);
                _tables[tableName].SetEntityState(changedEntity, EntityState.Synchronized);
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

                if (!_tables.ContainsKey(tableName))
                {
                    IEnumerable<T> tableContent = fileHelper.ReadTable<T>(tablePath);
                    var table = new DbTable<Entity>(tableContent);
                    _tables.Add(tableName, table);
                }

                _tables[tableName].Remove(new T() { Id = id });
            }
        }
    }
}
