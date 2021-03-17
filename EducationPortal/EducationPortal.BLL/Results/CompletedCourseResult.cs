namespace EducationPortal.BLL.Results
{
    using System.Collections.Generic;
    using EducationPortal.BLL.DTO;

    public class CompletedCourseResult : OperationResult
    {
        public IDictionary<SkillDTO, int> RecievedSkills { get; set; }
    }
}
