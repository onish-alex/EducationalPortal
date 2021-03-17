namespace EducationPortal.BLL.Results
{
    using System.Collections.Generic;
    using EducationPortal.BLL.DTO;
    using EducationPortal.BLL.Utilities;

    public class GetMaterialPageResult : OperationResult
    {
        public PaginatedList<MaterialDTO> Materials { get; set; }

        public IDictionary<MaterialDTO, bool> MaterialStatuses { get; set; }
    }
}
