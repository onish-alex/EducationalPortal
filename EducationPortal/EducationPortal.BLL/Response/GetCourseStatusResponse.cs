namespace EducationPortal.BLL.Response
{
    public class GetCourseStatusResponse : IResponse
    {
        public string Message { get; set; }

        public string CreatorName { get; set; }

        public bool IsSuccessful { get; set; }

        public bool IsCreator { get; set; }

        public bool IsJoined { get; set; }

        public bool IsCompleted { get; set; }
    }
}
