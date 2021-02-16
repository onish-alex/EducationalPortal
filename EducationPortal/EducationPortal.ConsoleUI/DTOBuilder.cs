namespace EducationPortal.ConsoleUI
{
    using EducationPortal.BLL.DTO;

    public class DTOBuilder
    {
        private static DTOBuilder instance = new DTOBuilder();

        private DTOBuilder()
        {
        }

        public static DTOBuilder GetInstance()
        {
            return instance;
        }

        public UserDTO GetUser(string[] parts)
        {
            return new UserDTO() { Name = parts[0] };
        }

        public AccountDTO GetAccount(string[] parts, bool isReg = false)
        {
            return isReg ? new AccountDTO() { Email = parts[0], Login = parts[1], Password = parts[2] }
                         : new AccountDTO() { Email = parts[0], Login = parts[0], Password = parts[1] };
        }

        public SkillDTO GetSkill(string[] parts)
        {
            return new SkillDTO() { Name = parts[0] };
        }

        public BookDTO GetBook(string name, string url, string authors, string pageCount, string format, string publishingYear)
        {
            return new BookDTO()
            {
                AuthorNames = authors,
                PageCount = pageCount,
                Format = format,
                PublishingYear = publishingYear,
                Name = name,
                Url = url,
            };
        }

        public ArticleDTO GetArticle(string name, string url, string publicationDate)
        {
            return new ArticleDTO()
            {
                Name = name,
                Url = url,
                PublicationDate = publicationDate,
            };
        }

        public VideoDTO GetVideo(string name, string url, string duration, string quality)
        {
            return new VideoDTO()
            {
                Name = name,
                Url = url,
                Duration = duration,
                Quality = quality,
            };
        }
    }
}
