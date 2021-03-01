namespace EducationPortal.BLL.Response
{
    using System;
    using System.Collections.Generic;
    using System.Text;

    public class OperationResult : IOperationResult
    {
        public OperationResult()
        {
        }

        public OperationResult(string message, bool isSuccessful)
        {
            this.MessageCode = message;
            this.IsSuccessful = isSuccessful;
        }

        public string MessageCode { get; set; }

        public bool IsSuccessful { get; set; }
    }
}
