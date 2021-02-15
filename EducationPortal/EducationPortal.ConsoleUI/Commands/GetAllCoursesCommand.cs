using System;
using System.Collections.Generic;
using System.Text;
using EducationPortal.BLL.DTO;
using EducationPortal.BLL.Response;
using EducationPortal.BLL.Services;

namespace EducationPortal.ConsoleUI.Commands
{
    public class GetAllCoursesCommand : ICommand<GetCoursesResponse>
    {
        private ICourseService courseService;

        public GetCoursesResponse Response { get; set; }

        public GetAllCoursesCommand(ICourseService courseService)
        {
            this.courseService = courseService;
        }

        public void Execute()
        {
            Response = courseService.GetAllCourses();
        }
    }
}
