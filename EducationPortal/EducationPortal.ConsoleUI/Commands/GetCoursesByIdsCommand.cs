using EducationPortal.BLL.Response;
using EducationPortal.BLL.Services;
using System;
using System.Collections.Generic;
using System.Text;

namespace EducationPortal.ConsoleUI.Commands
{
    public class GetCoursesByIdsCommand : ICommand<GetCoursesResponse>
    {
        private ICourseService courseService;
        private long[] ids; 

        public GetCoursesResponse Response { get; set; }

        public GetCoursesByIdsCommand(ICourseService courseService, long[] ids)
        {
            this.courseService = courseService;
            this.ids = ids;
        }

        public void Execute()
        {
            Response = courseService.GetByIds(ids);
        }
    }
}
