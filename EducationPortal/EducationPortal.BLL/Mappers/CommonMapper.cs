namespace EducationPortal.BLL.Mappers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using AutoMapper;
    using EducationPortal.BLL.DTO;
    using EducationPortal.DAL.Entities.EF;

    public class CommonMapper
    {
        private static CommonMapper instance = new CommonMapper();
        private Mapper mapper;
        private Mapper skillMapper;

        private CommonMapper()
        {
            this.skillMapper = new Mapper(new MapperConfiguration(cfg => cfg.CreateMap<SkillDTO, Skill>().ReverseMap()));

            var mapperConfig = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<AccountDTO, Account>();
                cfg.CreateMap<UserDTO, User>().ReverseMap();
                cfg.CreateMap<SkillDTO, Skill>().ReverseMap();

                cfg.CreateMap<Course, CourseDTO>()
                   .ForMember(
                       dest => dest.Skills,
                       opt => opt.MapFrom(entity => this.skillMapper.Map<IEnumerable<SkillDTO>>(entity.Skills)));

                cfg.CreateMap<CourseDTO, Course>();

                cfg.CreateMap<MaterialDTO, Material>().ReverseMap();

                cfg.CreateMap<BookDTO, Book>().ReverseMap();
                cfg.CreateMap<ArticleDTO, Article>().ReverseMap();
                cfg.CreateMap<VideoDTO, Video>().ForMember(
                    dest => dest.Duration,
                    opt => opt.MapFrom(dto => TimeSpan.FromSeconds(int.Parse(dto.Duration))));

                cfg.CreateMap<Video, VideoDTO>().ForMember(
                    dest => dest.Duration,
                    opt => opt.MapFrom(entity => entity.Duration.TotalSeconds.ToString()));
            });

            this.mapper = new Mapper(mapperConfig);
        }

        public static CommonMapper GetInstance()
        {
            return instance;
        }

        public TDestination Map<TSource, TDestination>(TSource source)
            where TSource : class
            where TDestination : class
        {
            switch (source)
            {
                case Skill _:
                case SkillDTO _:
                    return this.skillMapper.Map<TDestination>(source);

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
                case IEnumerable<Skill> _:
                case IEnumerable<SkillDTO> _:
                    return this.skillMapper.Map<IEnumerable<TDestination>>(sourceCollection);

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
