using System;

namespace EducationPortal.DAL.Entities
{
    public abstract class Entity : ICloneable
    {
        public long Id { get; set; }

        public object Clone()
        {
            return MemberwiseClone();
        }
    }
}
