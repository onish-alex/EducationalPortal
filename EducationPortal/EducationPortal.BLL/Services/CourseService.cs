namespace EducationPortal.BLL.Services
{
    using System;
    using System.Linq;
    using EducationPortal.BLL.DTO;
    using EducationPortal.BLL.Mappers;
    using EducationPortal.BLL.Response;
    using EducationPortal.DAL.Entities.EF;
    using EducationPortal.DAL.Repository.Base;

    public class CourseService : ICourseService
    {
        private IRepository<Course> courses;
        private IRepository<Skill> skills;
        private IRepository<Material> materials;
        private CommonMapper mapper;

        public CourseService(
            IRepository<Course> courses,
            IRepository<Skill> skills,
            IRepository<Material> materials)
        {
            this.courses = courses;
            this.skills = skills;
            this.materials = materials;

            this.mapper = CommonMapper.GetInstance();
        }

        public string Name => "Course";

        public OperationResponse AddCourse(CourseDTO course)
        {
            var response = new OperationResponse();
            var courseToAdd = this.mapper.Map<CourseDTO, Course>(course);

            this.courses.Create(courseToAdd);
            this.courses.Save();

            response.Message = "Новый курс успешно создан!";
            return response;
        }

        public GetCoursesResponse GetUserCourses(long userId)
        {
            var response = new GetCoursesResponse();
            var userCourses = this.courses.Find(course => course.CreatorId == userId);
            response.Courses = this.mapper.Map<Course, CourseDTO>(userCourses);
            return response;
        }

        public GetCoursesResponse GetAllCourses()
        {
            var response = new GetCoursesResponse();
            var userCourses = this.courses.GetAll();
            response.Courses = this.mapper.Map<Course, CourseDTO>(userCourses);
            return response;
        }

        public OperationResponse EditCourse(long userId, CourseDTO newCourseInfo)
        {
            var response = this.CanEditCourse(userId, newCourseInfo.Id);

            if (!response.IsSuccessful)
            {
                return response;
            }

            var courseToUpdate = this.courses.GetById(newCourseInfo.Id);
            courseToUpdate.Name = newCourseInfo.Name;
            courseToUpdate.Description = newCourseInfo.Description;

            this.courses.Update(courseToUpdate);
            this.courses.Save();

            response.Message = "Курс успешно обновлен";
            response.IsSuccessful = true;

            return response;
        }

        public OperationResponse AddSkill(long userId, long courseId, SkillDTO skill)
        {
            var response = this.CanEditCourse(userId, courseId);

            if (!response.IsSuccessful)
            {
                return response;
            }

            var course = this.courses.GetById(courseId);

            var skillToAdd = this.skills.Find(sk => sk.Name == skill.Name).SingleOrDefault();

            if (skillToAdd == null)
            {
                skillToAdd = this.mapper.Map<SkillDTO, Skill>(skill);
                this.skills.Create(skillToAdd);
                this.skills.Save();
            }
            else if (course.Skills.Contains(skillToAdd))
            {
                response.IsSuccessful = false;
                response.Message = "Курс уже содержит такое умение!";
                return response;
            }

            course.Skills.Add(skillToAdd);

            this.courses.Update(course);
            this.courses.Save();

            response.IsSuccessful = true;
            response.Message = "Умение успешно добавлено!";

            return response;
        }

        public OperationResponse RemoveSkill(long userId, long courseId, SkillDTO skill)
        {
            var response = this.CanEditCourse(userId, courseId);

            if (!response.IsSuccessful)
            {
                return response;
            }

            var course = this.courses.GetById(courseId);

            var skillToRemove = course.Skills.FirstOrDefault(x => x.Name == skill.Name);

            if (skillToRemove == null)
            {
                response.Message = "У выбранного курса нет указанного умения!";
                response.IsSuccessful = false;
                return response;
            }

            course.Skills.Remove(skillToRemove);
            this.courses.Update(course);
            this.courses.Save();

            response.IsSuccessful = true;
            response.Message = "Умение успешно удалено!";

            return response;
        }

        public OperationResponse AddMaterialToCourse(long userId, long courseId, long materialId)
        {
            var response = this.CanEditCourse(userId, courseId);

            if (!response.IsSuccessful)
            {
                return response;
            }

            var course = this.courses.GetById(courseId);

            if (course.Materials.Select(x => x.Id).Contains((int)materialId))
            {
                response.Message = "Данный курс уже содержит этот материал!";
                response.IsSuccessful = false;
                return response;
            }

            var material = this.materials.GetById(materialId);

            course.Materials.Add(material);

            this.courses.Update(course);
            this.courses.Save();

            response.IsSuccessful = true;
            response.Message = "Материал успешно добавлен к курсу!";

            return response;
        }

        public OperationResponse CanEditCourse(long userId, long courseId)
        {
            var response = new OperationResponse();

            var course = this.courses.GetById(courseId);

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

            var course = this.courses.GetById(courseId);

            if (course == null)
            {
                response.IsSuccessful = false;
                response.Message = "Данного курса не существует!";
                return response;
            }

            if (course.JoinedUsers.Any(x => x.UserId == userId))
            {
                response.IsSuccessful = false;
                response.Message = "Вы уже участник данного курса!";
                return response;
            }

            if (course.CompletedUsers.Any(x => x.UserId == userId))
            {
                response.IsSuccessful = false;
                response.Message = "Вы уже прошли данный курс!";
                return response;
            }

            response.IsSuccessful = true;
            return response;
        }

        public GetCourseStatusResponse GetCourseStatus(long courseId, long userId)
        {
            var response = new GetCourseStatusResponse();

            var course = this.courses.GetById(courseId);

            if (course == null)
            {
                response.IsSuccessful = false;
                response.Message = "Указанного курса не существует!";
            }

            if (course.CreatorId == userId)
            {
                response.IsCreator = true;
            }

            if (course.CompletedUsers.Select(x => x.UserId).Contains((int)userId))
            {
                response.IsCompleted = true;
            }

            if (course.JoinedUsers.Select(x => x.UserId).Contains((int)userId))
            {
                response.IsJoined = true;
            }

            response.CreatorName = course.Creator.Name;
            response.Skills = this.mapper.Map<Skill, SkillDTO>(course.Skills);

            response.IsSuccessful = true;

            return response;
        }
    }
}
