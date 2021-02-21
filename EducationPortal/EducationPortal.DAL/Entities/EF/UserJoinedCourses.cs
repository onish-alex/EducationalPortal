﻿namespace EducationPortal.DAL.Entities.EF
{
    public class UserJoinedCourses
    {
        public long UserId { get; set; }

        public long CourseId { get; set; }

        public User User { get; set; }

        public Course Course { get; set; }
    }
}
