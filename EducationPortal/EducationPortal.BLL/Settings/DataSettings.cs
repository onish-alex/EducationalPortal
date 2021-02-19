namespace EducationPortal.BLL.Settings
{
    public static class DataSettings
    {
        public const string AllowableSymbols = "abcdefghijklmnopqrstuvwxyzабвгдеёжхийклмнопрстуфхцчшщъыьэюя1234567890_";

        public const int UserLoginMinCharacterCount = 3;
        public const int UserLoginMaxCharacterCount = 20;

        public const int UserNameMinCharacterCount = 3;
        public const int UserNameMaxCharacterCount = 50;

        public const int UserPasswordMinCharacterCount = 3;
        public const int UserPasswordMaxCharacterCount = 20;

        public const int CourseNameMinCharacterCount = 3;
        public const int CourseNameMaxCharacterCount = 100;

        public const int CourseDescriptionMinCharacterCount = 3;
        public const int CourseDescriptionMaxCharacterCount = 250;

        public const int SkillNameMinCharacterCount = 2;
        public const int SkillNameMaxCharacterCount = 30;

        public const int MaterialNameMinCharacterCount = 3;
        public const int MaterialNameMaxCharacterCount = 20;
    }
}
