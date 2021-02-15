using System;
using System.Collections.Generic;
using System.Text;
using EducationPortal.BLL.Response;
using EducationPortal.BLL.DTO;
using EducationPortal.BLL.Services;

namespace EducationPortal.ConsoleUI.Commands
{
    public class GetUserCoursesCommand : ICommand<GetCoursesResponse>
    {
        private ICourseService courseService;

        public GetCoursesResponse Response { get; set; }

        private long userId;

        public GetUserCoursesCommand(ICourseService courseService, long userId)
        {
            this.courseService = courseService;
            this.userId = userId;
        }

        public void Execute()
        {
            Response = courseService.GetUserCourses(userId);
        }
    }
}
