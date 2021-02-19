namespace EducationPortal.BLL.Mappers
{
    using System.Collections.Generic;

    public interface IMapper
    {
        public TDestination Map<TSource, TDestination>(TSource source)
            where TSource : class
            where TDestination : class;

        public IEnumerable<TDestination> Map<TSource, TDestination>(IEnumerable<TSource> sourceCollection)
            where TSource : class
            where TDestination : class;
    }
}
