namespace EducationPortal.BLL.Response
{
    using System.Collections.Generic;
    using EducationPortal.BLL.DTO;

    public class GetMaterialsResult : OperationResult
    {
        public IEnumerable<MaterialDTO> Materials { get; set; }
    }
}
