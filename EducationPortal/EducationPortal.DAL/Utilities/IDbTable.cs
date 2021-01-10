using System;
using System.Collections.Generic;
using EducationPortal.DAL.Entities;

namespace EducationPortal.DAL.Utilities
{
    public interface IDbTable
    {
        public IEnumerable<Entity> Content { get; }

        public void Add(Entity item);

        public void Update(Entity item);

        public void Remove(Entity item);

        public IEnumerable<Entity> Find(Func<Entity, bool> predicate);
    }
}
