using System;
using System.Collections.Generic;
using System.Text;

namespace EducationPortal.BLL.DTO
{
    public class VideoDTO : MaterialDTO
    {
        public string Duration { get; set; }

        public string Quality { get; set; }

        public override string ToString()
        {
            return base.ToString() + string.Format("\nПродолжительность: {0}\nКачество: {1}\nURL: {2}",
                                                   string.Format("{0:d2}:{1:d2}", int.Parse(Duration) / 60, int.Parse(Duration) % 60), Quality, Url);


        }
    }
}
