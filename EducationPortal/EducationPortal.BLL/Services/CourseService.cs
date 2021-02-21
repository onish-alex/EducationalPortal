namespace EducationPortal.BLL.Services
{
    using System;
    using System.Linq;
    using EducationPortal.BLL;
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
        private IMapper mapper;

        public CourseService(
            IRepository<Course> courses,
            IRepository<Skill> skills,
            IRepository<Material> materials,
            IMapper mapper)
        {
            this.courses = courses;
            this.skills = skills;
            this.materials = materials;
            this.mapper = mapper;
        }

        public string Name => "Course";

        public OperationResponse AddCourse(CourseDTO course)
        {
            var response = new OperationResponse(ResponseMessages.AddCourseSuccess, true);
            var courseToAdd = this.mapper.Map<CourseDTO, Course>(course);

            this.courses.Create(courseToAdd);
            this.courses.Save();

            return response;
        }

        public GetCoursesResponse GetUserCourses(long userId)
        {
            var response = new GetCoursesResponse();

            var userCourses = this.courses.Find(
                course => course.CreatorId == userId,
                course => course.Skills);

            response.Courses = this.mapper.Map<Course, CourseDTO>(userCourses);

            return response;
        }

        public GetCoursesResponse GetAllCourses()
        {
            var response = new GetCoursesResponse();

            var userCourses = this.courses.GetAll(course => course.Skills);
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

            response.Message = ResponseMessages.EditCourseSuccess;
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

            var course = this.courses.Find(
                course => course.Id == courseId,
                course => course.Skills)
                .SingleOrDefault();

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
                response.Message = ResponseMessages.AddSkillAlreadyExists;
                return response;
            }

            course.Skills.Add(skillToAdd);

            this.courses.Update(course);
            this.courses.Save();

            response.IsSuccessful = true;
            response.Message = ResponseMessages.AddSkillSuccess;

            return response;
        }

        public OperationResponse RemoveSkill(long userId, long courseId, SkillDTO skill)
        {
            var response = this.CanEditCourse(userId, courseId);

            if (!response.IsSuccessful)
            {
                return response;
            }

            var course = this.courses.Find(
                course => course.Id == courseId,
                course => course.Skills)
                .SingleOrDefault();

            var skillToRemove = course.Skills.FirstOrDefault(x => x.Name == skill.Name);

            if (skillToRemove == null)
            {
                response.Message = ResponseMessages.RemoveSkillNotFound;
                response.IsSuccessful = false;
                return response;
            }

            course.Skills.Remove(skillToRemove);
            this.courses.Update(course);
            this.courses.Save();

            response.IsSuccessful = true;
            response.Message = ResponseMessages.RemoveSkillSuccess;

            return response;
        }

        public OperationResponse AddMaterialToCourse(long userId, long courseId, long materialId)
        {
            var response = this.CanEditCourse(userId, courseId);

            if (!response.IsSuccessful)
            {
                return response;
            }

            var course = this.courses.Find(
                            course => course.Id == courseId,
                            course => course.Materials)
                            .SingleOrDefault();

            if (course.Materials.Any(x => x.Id == materialId))
            {
                response.Message = ResponseMessages.AddMaterialToCourseAlreadyExists;
                response.IsSuccessful = false;
                return response;
            }

            var material = this.materials.GetById(materialId);

            course.Materials.Add(material);

            this.courses.Update(course);
            this.courses.Save();

            response.IsSuccessful = true;
            response.Message = ResponseMessages.AddMaterialToCourseSuccess;

            return response;
        }

        public OperationResponse CanEditCourse(long userId, long courseId)
        {
            var response = new OperationResponse();

            var course = this.courses.GetById(courseId);

            if (course == null)
            {
                response.Message = ResponseMessages.CourseNotFound;
                response.IsSuccessful = false;
                return response;
            }

            if (course.CreatorId != userId)
            {
                response.Message = ResponseMessages.CanEditCourseNotAnAuthor;
                response.IsSuccessful = false;
                return response;
            }

            response.IsSuccessful = true;
            return response;
        }

        public OperationResponse CanJoinCourse(long userId, long courseId)
        {
            var response = new OperationResponse();

            var course = this.courses.Find(
                course => course.Id == courseId,
                course => course.JoinedUsers,
                course => course.CompletedUsers)
                .SingleOrDefault();

            if (course == null)
            {
                response.IsSuccessful = false;
                response.Message = ResponseMessages.CourseNotFound;
                return response;
            }

            if (course.JoinedUsers.Any(x => x.UserId == userId))
            {
                response.IsSuccessful = false;
                response.Message = ResponseMessages.CanJoinCourseAlreadyJoin;
                return response;
            }

            if (course.CompletedUsers.Any(x => x.UserId == userId))
            {
                response.IsSuccessful = false;
                response.Message = ResponseMessages.CourseAlreadyCompleted;
                return response;
            }

            response.IsSuccessful = true;
            return response;
        }

        public GetCourseStatusResponse GetCourseStatus(long courseId, long userId)
        {
            var response = new GetCourseStatusResponse();

            var course = this.courses.Find(
                course => course.Id == courseId,
                course => course.CompletedUsers,
                course => course.JoinedUsers,
                course => course.Creator,
                course => course.Skills)
                .SingleOrDefault();

            if (course == null)
            {
                response.IsSuccessful = false;
                response.Message = ResponseMessages.CourseNotFound;
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
