﻿using EducationPortal.DAL.Entities;
using System;
using System.Collections.Generic;

namespace EducationPortal.DAL.Repository
{
    public class UserRepository : IUserRepository
    {
        private IRepository<User> baseRepository;
        public UserRepository(IRepository<User> baseRepository)
        {
            this.baseRepository = baseRepository;
        }

        public void Create(User item)
        {
            baseRepository.Create(item);
        }

        public void Delete(long id)
        {
            baseRepository.Delete(id);
        }

        public IEnumerable<User> Find(Func<User, bool> predicate)
        {
            return baseRepository.Find(predicate);
        }

        public IEnumerable<User> GetAll()
        {
            return baseRepository.GetAll();
        }

        public User GetById(long id)
        {
            return baseRepository.GetById(id);
        }

        public void Update(User item)
        {
            baseRepository.Update(item);
        }
    }
}
