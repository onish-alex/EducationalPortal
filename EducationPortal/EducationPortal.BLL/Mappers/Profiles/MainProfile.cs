namespace EducationPortal.BLL.Mappers.Profiles
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using AutoMapper;
    using EducationPortal.BLL.DTO;
    using EducationPortal.DAL.Entities.EF;

    public class MainProfile : Profile
    {
        public MainProfile()
        {
            this.CreateMap<Course, CourseDTO>().ReverseMap();
            this.CreateMap<AccountDTO, Account>();
            this.CreateMap<UserDTO, User>().ReverseMap();
            this.CreateMap<SkillDTO, Skill>().ReverseMap();

            this.CreateMap<MaterialDTO, Material>().ReverseMap();

            this.CreateMap<BookDTO, Book>().ReverseMap();

            this.CreateMap<ArticleDTO, Article>();
            this.CreateMap<Article, ArticleDTO>().ForMember(
                dest => dest.PublicationDate,
                opt => opt.MapFrom(entity => entity.PublicationDate.ToShortDateString()));

            this.CreateMap<VideoDTO, Video>().ForMember(
                dest => dest.Duration,
                opt => opt.MapFrom(dto => TimeSpan.FromSeconds(int.Parse(dto.Duration))));

            this.CreateMap<Video, VideoDTO>().ForMember(
                dest => dest.Duration,
                opt => opt.MapFrom(entity => entity.Duration.TotalSeconds.ToString()));
        }
    }
}
