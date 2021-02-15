using AutoMapper;
using EducationPortal.BLL.DTO;
using EducationPortal.BLL.Response;
using EducationPortal.DAL.Entities;
using EducationPortal.DAL.Repository;
using System;
using System.Linq;
using System.Collections.Generic;

namespace EducationPortal.BLL.Services
{
    public class MaterialService : IMaterialService
    {
        private IRepository<Material> materials;
        private Mapper materialMapper;

        public string Name => "Material";

        public MaterialService(IRepository<Material> materials)
        {
            this.materials = materials;

            var materialConfig = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<Material, MaterialDTO>().ReverseMap();

                cfg.CreateMap<ArticleDTO, Article>()
                .ForMember(dest => dest.PublicationDate,
                           opt => opt.MapFrom(dto => DateTime.Parse(dto.PublicationDate))); 

                cfg.CreateMap<Article, ArticleDTO>()
                .ForMember(dest => dest.PublicationDate,
                           opt => opt.MapFrom(entity => entity.PublicationDate.ToString("yyyy-MM-dd")));

                cfg.CreateMap<Book, BookDTO>()
                .ForMember(dest => dest.PageCount,
                           opt => opt.MapFrom(entity => entity.PageCount.ToString()))
                .ForMember(dest => dest.PublishingYear,
                           opt => opt.MapFrom(entity => entity.PublishingYear.ToString()));

                cfg.CreateMap<BookDTO, Book>()
                .ForMember(dest => dest.PageCount,
                           opt => opt.MapFrom(dto => int.Parse(dto.PageCount)))
                .ForMember(dest => dest.PublishingYear,
                           opt => opt.MapFrom(dto => int.Parse(dto.PublishingYear)));

                cfg.CreateMap<Video, VideoDTO>()
                .ForMember(dest => dest.Duration,
                           opt => opt.MapFrom(entity => entity.Duration.ToString()));

                cfg.CreateMap<VideoDTO, Video>()
                .ForMember(dest => dest.Duration,
                           opt => opt.MapFrom(dto => int.Parse(dto.Duration)));
            });

            materialMapper = new Mapper(materialConfig);
        }

        public AddMaterialResponse AddMaterial(MaterialDTO material)
        {
            var response = new AddMaterialResponse();

            var entityType = materialMapper.ConfigurationProvider
                                           .GetAllTypeMaps()
                                           .FirstOrDefault(a => a.SourceType == material.GetType())
                                           .DestinationType;

            var materialToAdd = materialMapper.Map(material, material.GetType(), entityType) as Material;
            materialToAdd.MaterialType = entityType.Name;

            materials.Create(materialToAdd);
            materials.Save();

            response.MaterialId = materialToAdd.Id;
            response.IsSuccessful = true;

            return response;
        }

        public GetMaterialsResponse GetAllMaterials()
        {
            var response = new GetMaterialsResponse();

            var allMaterials = materials.GetAll();

            var allMaterialDTOs = new List<MaterialDTO>();

            foreach (var item in allMaterials)
            {
                var entityType = materialMapper.ConfigurationProvider
                                           .GetAllTypeMaps()
                                           .FirstOrDefault(a => a.SourceType == item.GetType())
                                           .DestinationType;

                var materialToAdd = materialMapper.Map(item, item.GetType(), entityType);

                allMaterialDTOs.Add(materialToAdd as MaterialDTO);
            }

            response.IsSuccessful = allMaterialDTOs.Count != 0;
            response.Materials = allMaterialDTOs;

            return response;
        }

        public GetMaterialsResponse GetByIds(long[] ids)
        {
            var response = new GetMaterialsResponse();

            var materialsByIds = materials.Find(x => ids.Contains(x.Id));

            var materialDTOs = new List<MaterialDTO>();

            foreach (var item in materialsByIds)
            {
                var entityType = materialMapper.ConfigurationProvider
                                           .GetAllTypeMaps()
                                           .FirstOrDefault(a => a.SourceType == item.GetType())
                                           .DestinationType;

                var materialToAdd = materialMapper.Map(item, item.GetType(), entityType);

                materialDTOs.Add(materialToAdd as MaterialDTO);
            }

            response.Materials = materialDTOs;
            response.IsSuccessful = materialDTOs.Count != 0;

            return response;
        }
    }
}
