namespace EducationPortal.BLL.Settings
{
    using System;
    using System.Linq;
    using AutoMapper;
    using EducationPortal.BLL.DTO;
    using EducationPortal.DAL.Entities.EF;

    public static class MappingConfigurations
    {
        public static MapperConfiguration Configurations { get; } = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<AccountDTO, Account>();
                cfg.CreateMap<UserDTO, User>().ReverseMap();
                cfg.CreateMap<SkillDTO, Skill>().ReverseMap();

                cfg.CreateMap<Course, CourseDTO>()
                   .ForMember(
                       dest => dest.SkillNames,
                       opt => opt.MapFrom(entity => entity.Skills.Select(skill => skill.Name)));

                cfg.CreateMap<CourseDTO, Course>();

                cfg.CreateMap<MaterialDTO, Material>().ReverseMap();

                cfg.CreateMap<BookDTO, Book>().ReverseMap();

                cfg.CreateMap<ArticleDTO, Article>();
                cfg.CreateMap<Article, ArticleDTO>().ForMember(
                    dest => dest.PublicationDate,
                    opt => opt.MapFrom(entity => entity.PublicationDate.ToShortDateString()));

                cfg.CreateMap<VideoDTO, Video>().ForMember(
                    dest => dest.Duration,
                    opt => opt.MapFrom(dto => TimeSpan.FromSeconds(int.Parse(dto.Duration))));

                cfg.CreateMap<Video, VideoDTO>().ForMember(
                    dest => dest.Duration,
                    opt => opt.MapFrom(entity => entity.Duration.TotalSeconds.ToString()));
            });
    }
}
