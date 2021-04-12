namespace EducationPortal.DAL.Entities.EF
{
    using System.Collections.Generic;

    public class Material
    {
        public long Id { get; set; }

        public string Name { get; set; }

        public string Url { get; set; }

        public virtual IEnumerable<Course> Courses { get; set; }

        public virtual IEnumerable<User> Users { get; set; }
    }
}
