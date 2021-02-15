using System;
using System.Collections.Generic;
using System.Text;

namespace EducationPortal.BLL.Response
{
    public class OperationResponse : IResponse
    {
        public string Message { get; set; }
        public bool IsSuccessful { get; set; }
    }
}
