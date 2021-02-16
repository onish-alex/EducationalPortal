namespace EducationPortal.BLL.Response
{
    using System;
    using System.Collections.Generic;
    using System.Text;

    public class OperationResponse : IResponse
    {
        public string Message { get; set; }

        public bool IsSuccessful { get; set; }
    }
}
