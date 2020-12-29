using System;

namespace EducationalPortal.DAL.Entities
{
    public abstract class Entity : ICloneable
    {
        public long id { get; set; }

        public object Clone()
        {
            return MemberwiseClone();
        }
    }
}
