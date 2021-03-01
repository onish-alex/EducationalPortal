namespace EducationPortal.BLL.Response
{
    using System;
    using System.Collections.Generic;
    using System.Text;

    public class OperationResponse : IResponse
    {
        public OperationResponse()
        {
        }

        public OperationResponse(string message, bool isSuccessful)
        {
            this.Message = message;
            this.IsSuccessful = isSuccessful;
        }

        public string Message { get; set; }

        public bool IsSuccessful { get; set; }
    }
}
