using EducationalPortal.DAL.Entities;
using System;
using System.Collections.Generic;

namespace EducationalPortal.DAL.Repository
{
    public class RepositoryDecorator<T> : IRepository<T> where T : Entity
    {
        private IRepository<T> _baseRepository;
        public RepositoryDecorator(IRepository<T> baseRepository)
        {
            _baseRepository = baseRepository;
        }

        public void Create(T item) => _baseRepository.Create(item);

        public void Delete(long id) => _baseRepository.Delete(id);

        public IEnumerable<T> Find(Func<T, bool> predicate) => _baseRepository.Find(predicate);

        public IEnumerable<T> GetAll() => _baseRepository.GetAll();

        public T GetById(long id) => _baseRepository.GetById(id);

        public void Update(T item) => _baseRepository.Update(item);
    }
}
