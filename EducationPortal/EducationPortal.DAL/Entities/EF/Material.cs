namespace EducationPortal.DAL.Entities.EF
{
    using System.Collections.Generic;

    public class Material
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string Url { get; set; }

        public IEnumerable<Course> Courses { get; set; }

        public IEnumerable<User> Users { get; set; }
    }
}
