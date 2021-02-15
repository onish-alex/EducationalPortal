using System;
using System.Collections.Generic;
using System.Text;

namespace EducationPortal.BLL.DTO
{
    public class MaterialDTO
    {
        public long Id { get; set; }

        public string Name { get; set; }
        
        public string Url { get; set; }

        public override string ToString()
        {
            return string.Format("\"{0}\"", Name);
        }
    }
}
