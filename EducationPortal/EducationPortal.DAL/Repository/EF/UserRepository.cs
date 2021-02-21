namespace EducationPortal.DAL.Repository.EF
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Linq.Expressions;
    using EducationPortal.DAL.DbContexts;
    using EducationPortal.DAL.Entities.EF;
    using Microsoft.EntityFrameworkCore;

    public class UserRepository : EFRepository<User>
    {
        public UserRepository(EFDbContext db)
            : base(db)
        {
        }

        public override IEnumerable<User> Find(Expression<Func<User, bool>> predicate, params Expression<Func<User, object>>[] includes)
        {
            return this.Get(includes).Where(predicate).ToList();
        }

        public override IEnumerable<User> GetAll(params Expression<Func<User, object>>[] includes)
        {
            return this.Get(includes).ToList();
        }

        protected override IQueryable<User> Get(params Expression<Func<User, object>>[] includes)
        {
            IQueryable<User> result = this.db.Users;
            foreach (var include in includes)
            {
                if (include.Body.Type == typeof(ICollection<UserJoinedCourses>))
                {
                    result = result.Include(include)
                                   .ThenInclude(x => (x as UserJoinedCourses).Course);
                }
                else if (include.Body.Type == typeof(ICollection<UserCompletedCourses>))
                {
                    result = result.Include(include)
                                   .ThenInclude(x => (x as UserCompletedCourses).Course);
                }
                else if (include.Body.Type == typeof(ICollection<UserSkills>))
                {
                    result = result.Include(include)
                                   .ThenInclude(x => (x as UserSkills).Skill);
                }
                else
                {
                    result = result.Include(include);
                }
            }

            return result;
        }
    }
}
