namespace EducationPortal.DAL.Entities.EF
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Linq;

    public class User
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public ICollection<UserJoinedCourses> JoinedCourses { get; set; }

        public ICollection<UserCompletedCourses> CompletedCourses { get; set; }

        public ICollection<Material> LearnedMaterials { get; set; }

        [NotMapped]
        public IDictionary<Skill, int> SkillsLevels => this.UserSkills.ToDictionary(k => k.Skill, v => v.Level);

        public ICollection<Course> CreatedCourses { get; set; }

        public ICollection<UserSkills> UserSkills { get; set; }

        public Account Account { get; set; }
    }
}
