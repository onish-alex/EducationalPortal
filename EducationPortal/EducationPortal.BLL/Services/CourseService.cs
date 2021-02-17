using EducationPortal.BLL.DTO;
using EducationPortal.BLL.Response;
using System;
using System.Linq;
using System.Collections.Generic;
using EducationPortal.DAL.Repository;
using EducationPortal.DAL.Entities;
using AutoMapper;

namespace EducationPortal.BLL.Services
{
    public class CourseService : ICourseService
    {
        private IRepository<Course> courses;
        private IRepository<Skill> skills;
        private IRepository<Material> materials;

        private Mapper courseMapper;
        private Mapper skillMapper;

        public string Name => "Course";
        
        public CourseService(IRepository<Course> courses, 
                             IRepository<Skill> skills,
                             IRepository<Material> materials)
        {
            this.courses = courses;
            this.skills = skills;
            this.materials = materials;

            var skillConfig = new MapperConfiguration(cfg => cfg.CreateMap<SkillDTO, Skill>().ReverseMap());
            this.skillMapper = new Mapper(skillConfig);

            var courseConfig = new MapperConfiguration(cfg => 
            {
                cfg.CreateMap<CourseDTO, Course>()
                .ForMember(dest => dest.SkillIds,
                           opt => opt.MapFrom(dto => skills.Find(skill => skill.Name == dto.Name).Select(skill => skill.Id).ToArray()));

                cfg.CreateMap<Course, CourseDTO>()
                .ForMember(dest => dest.Skills, 
                           opt => opt.MapFrom(course => skillMapper.Map<IEnumerable<SkillDTO>>(skills.Find(a => course.SkillIds.Contains(a.Id)))));
            });
            this.courseMapper = new Mapper(courseConfig);
        }

        public OperationResponse AddCourse(CourseDTO course)
        {
            var response = new OperationResponse();
            var courseToAdd = courseMapper.Map<Course>(course);
            courseToAdd.MaterialIds = new long[0];
            courseToAdd.SkillIds = new long[0];
            courses.Create(courseToAdd);
            courses.Save();
            response.Message = "Новый курс успешно создан!";
            return response;
        }

        public GetCoursesResponse GetUserCourses(long userId)
        {
            var response = new GetCoursesResponse();

            var userCourses = courses.Find(course => course.CreatorId == userId);

            response.Courses = courseMapper.Map<IEnumerable<Course>, IEnumerable<CourseDTO>>(userCourses);

            return response;
        }

        public GetCoursesResponse GetAllCourses()
        {
            var response = new GetCoursesResponse();

            var userCourses = courses.GetAll();

            response.Courses = courseMapper.Map<IEnumerable<Course>, IEnumerable<CourseDTO>>(userCourses);

            return response;
        }

        public OperationResponse EditCourse(long userId, CourseDTO newCourseInfo)
        {
            var response = new OperationResponse();

            var courseToUpdate = courses.Find(course => course.CreatorId == userId
                                                     && newCourseInfo.Id == course.Id).SingleOrDefault();

            if (courseToUpdate == null)
            {
                response.Message = "Вы не являетесь автором данного курса";
                response.IsSuccessful = false;
                return response;
            }

            courseToUpdate.Name = newCourseInfo.Name;
            courseToUpdate.Description = newCourseInfo.Description;
            courses.Update(courseToUpdate);
            courses.Save();
            response.Message = "Курс успешно обновлен";
            response.IsSuccessful = true;

            return response;
        }

        public OperationResponse AddSkill(long userId, long courseId, SkillDTO skill)
        {
            var response = CanEditCourse(userId, courseId);

            if (!response.IsSuccessful)
            {
                return response;
            }

            var course = courses.GetById(courseId);

            var skillToAdd = skills.Find(sk => sk.Name == skill.Name).SingleOrDefault();

            if (skillToAdd == null)
            {
                skillToAdd = skillMapper.Map<SkillDTO, Skill>(skill);
                skills.Create(skillToAdd);
                skills.Save();
            } 
            else if (course.SkillIds.Contains(skillToAdd.Id))
            {
                response.IsSuccessful = false;
                response.Message = "Курс уже содержит такое умение!";
                return response;
            }

            course.SkillIds = course.SkillIds.Append(skillToAdd.Id).ToArray();

            courses.Update(course);
            courses.Save();

            response.IsSuccessful = true;
            response.Message = "Умение успешно добавлено!";

            return response;
        }

        public OperationResponse RemoveSkill(long userId, long courseId, SkillDTO skill)
        {
            var response = CanEditCourse(userId, courseId);

            if (!response.IsSuccessful)
            {
                return response;
            }

            var course = courses.GetById(courseId);

            var skillToRemove = skills.Find(sk => sk.Name == skill.Name).SingleOrDefault();

            if (skillToRemove == null 
             || !course.SkillIds.Contains(skillToRemove.Id))
            {
                response.Message = "У выбранного курса нет указанного умения!";
                response.IsSuccessful = false;
                return response;
            }

            course.SkillIds = course.SkillIds.Except(Enumerable.Repeat(skillToRemove.Id, 1)).ToArray();

            courses.Update(course);
            courses.Save();

            response.IsSuccessful = true;
            response.Message = "Умение успешно удалено!";

            return response;
        }

        public OperationResponse AddMaterialToCourse(long userId, long courseId, long materialId)
        {
            var response = CanEditCourse(userId, courseId);

            if (!response.IsSuccessful)
            {
                return response;
            }

            var course = courses.GetById(courseId);

            if (materials.GetById(materialId) == null)
            {
                response.Message = "Данного материала не существует!";
                response.IsSuccessful = false;
                return response;
            }

            if (course.MaterialIds.Contains(materialId))
            {
                response.Message = "Данный курс уже содержит этот материал!";
                response.IsSuccessful = false;
                return response;
            }

            course.MaterialIds = course.MaterialIds
                                       .Append(materialId)
                                       .ToArray();

            courses.Update(course);
            courses.Save();
            
            response.IsSuccessful = true;
            response.Message = "Материал успешно добавлен к курсу!";

            return response;
        }

        public OperationResponse CanEditCourse(long userId, long courseId)
        {
            var response = new OperationResponse();

            var course = courses.GetById(courseId);

            if (course == null)
            {
                response.Message = "Указанного курса не существует!";
                response.IsSuccessful = false;
                return response;
            }

            if (course.CreatorId != userId)
            {
                response.Message = "Вы не являетесь автором данного курса";
                response.IsSuccessful = false;
                return response;
            }

            response.IsSuccessful = true;
            return response;
        }

        public OperationResponse CanJoinCourse(long userId, long courseId)
        {
            var response = new OperationResponse();

            var course = courses.GetById(courseId);

            if (course == null)
            {
                response.IsSuccessful = false;
                response.Message = "Данного курса не существует!";
                return response;
            }

            //if (course.CreatorId == userId)
            //{
            //    response.IsSuccessful = false;
            //    response.Message = "Вы не можете проходить собственный курс!";
            //    return response;
            //} 

            response.IsSuccessful = true;
            return response;
        }

        public GetCoursesResponse GetByIds(long[] ids)
        {
            var response = new GetCoursesResponse();

            var coursesByIds = courses.Find(x => ids.Contains(x.Id));

            response.Courses = courseMapper.Map<IEnumerable<CourseDTO>>(coursesByIds);
            response.IsSuccessful = response.Courses.Count() != 0;

            return response;
        }

        public OperationResponse CanCompleteCourse(long courseId, long[] learnedMaterialIds)
        {
            var response = new GetCoursesResponse();

            var course = courses.GetById(courseId);

            if (course == null)
            {
                response.IsSuccessful = false;
                response.Message = "Данного курса не существует!";
                return response;
            }

            if (course.MaterialIds.Any(a => !learnedMaterialIds.Contains(a)))
            {
                response.IsSuccessful = false;
                response.Message = "Не все материалы данного курса изучены!";
            }

            response.IsSuccessful = true;

            return response;
        }

    }
}
