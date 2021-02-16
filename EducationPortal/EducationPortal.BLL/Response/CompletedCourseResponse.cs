namespace EducationPortal.BLL.Response
{
    using System.Collections.Generic;
    using EducationPortal.BLL.DTO;

    public class CompletedCourseResponse : OperationResponse
    {
        public IDictionary<SkillDTO, int> RecievedSkills { get; set; }
    }
}
