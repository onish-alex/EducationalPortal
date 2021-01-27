using EducationPortal.BLL.DTO;
using System;
using System.Collections.Generic;
using System.Text;

namespace EducationPortal.BLL.Response
{
    public class GetMaterialsResponse : OperationResponse
    {
        public IEnumerable<MaterialDTO> Materials { get; set; }
    }
}
