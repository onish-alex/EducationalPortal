namespace EducationPortal.BLL.Results
{
    using System.Collections.Generic;
    using EducationPortal.BLL.DTO;

    public class GetCourseStatusResult : OperationResult
    {
        public string CreatorName { get; set; }

        public bool IsCreator { get; set; }

        public bool IsJoined { get; set; }

        public bool IsCompleted { get; set; }

        public bool IsReadyToComplete { get; set; }

        public bool HasMaterials { get; set; }

        public IEnumerable<string> SkillNames { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }
    }
}
