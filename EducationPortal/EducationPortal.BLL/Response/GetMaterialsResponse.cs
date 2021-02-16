namespace EducationPortal.BLL.Response
{
    using System.Collections.Generic;
    using EducationPortal.BLL.DTO;

    public class GetMaterialsResponse : OperationResponse
    {
        public IEnumerable<MaterialDTO> Materials { get; set; }
    }
}
