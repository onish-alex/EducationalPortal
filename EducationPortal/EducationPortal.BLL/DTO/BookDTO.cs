namespace EducationPortal.BLL.DTO
{
    public class BookDTO : MaterialDTO
    {
        public string AuthorNames { get; set; }

        public string PageCount { get; set; }

        public string Format { get; set; }

        public string PublishingYear { get; set; }

        public override string ToString()
        {
            return base.ToString() + string.Format(
                "\nАвторы: {0}\nКоличество страниц: {1}\nФормат: {2}\nГод издания: {3}\nURL: {4}",
                this.AuthorNames,
                this.PageCount,
                this.Format,
                this.PublishingYear,
                this.Url);
        }
    }
}
