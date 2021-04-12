namespace EducationPortal.BLL.Mappers
{
    using System.Collections.Generic;
    using System.Linq;
    using AutoMapper;
    using EducationPortal.BLL.DTO;
    using EducationPortal.BLL.Settings;
    using EducationPortal.DAL.Entities.EF;

    public class EntityMapper : IMapper
    {
        private AutoMapper.IMapper mapper;

        public EntityMapper(AutoMapper.IMapper mapper)
        {
            this.mapper = mapper;
        }

        public TDestination Map<TSource, TDestination>(TSource source)
            where TSource : class
            where TDestination : class
        {
            switch (source)
            {
                case Material _:
                case MaterialDTO _:

                    var entityType = this.mapper.ConfigurationProvider
                                           .GetAllTypeMaps()
                                           .First(a => a.SourceType == source.GetType())
                                           .DestinationType;

                    return this.mapper.Map(source, source.GetType(), entityType) as TDestination;

                default:
                    return this.mapper.Map<TDestination>(source);
            }
        }

        public IEnumerable<TDestination> Map<TSource, TDestination>(IEnumerable<TSource> sourceCollection)
            where TSource : class
            where TDestination : class
        {
            switch (sourceCollection)
            {
                case IEnumerable<Material> _:
                case IEnumerable<MaterialDTO> _:

                    var entityList = new List<TDestination>();

                    foreach (var item in sourceCollection)
                    {
                        entityList.Add(this.Map<TSource, TDestination>(item));
                    }

                    return entityList;

                default:
                    return this.mapper.Map<IEnumerable<TDestination>>(sourceCollection);
            }
        }
    }
}
