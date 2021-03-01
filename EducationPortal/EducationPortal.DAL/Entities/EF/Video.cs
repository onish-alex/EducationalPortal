namespace EducationPortal.DAL.Entities.EF
{
    using System;

    public class Video : Material
    {
        public TimeSpan Duration { get; set; }

        public string Quality { get; set; }
    }
}
