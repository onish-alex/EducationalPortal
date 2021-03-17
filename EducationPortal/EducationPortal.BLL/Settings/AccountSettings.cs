namespace EducationPortal.BLL.Settings
{
    public class AccountSettings
    {
        public int LoginMinLength { get; set; }

        public int LoginMaxLength { get; set; }

        public int PasswordMinLength { get; set; }

        public int PasswordMaxLength { get; set; }

        public int NameMinLength { get; set; }

        public int NameMaxLength { get; set; }

        public string AllowableSymbols { get; set; }
    }
}
