namespace EducationPortal.DAL.Entities.File
{
    using System;

    public abstract class Entity : ICloneable
    {
        public long Id { get; set; }

        public object Clone()
        {
            return this.MemberwiseClone();
        }
    }
}
