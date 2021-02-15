using System;
using System.Collections.Generic;
using System.Text;

namespace EducationPortal.BLL.DTO
{
    public class ArticleDTO : MaterialDTO
    {
        public string PublicationDate { get; set; }

        public override string ToString()
        {
            return base.ToString() + string.Format("\nДата издания: {0}\nURL: {1}", PublicationDate, Url);
        }
    }
}
