namespace EducationPortal.ConsoleUI
{
    public static class ConsoleMessages
    {
        public static string DefaultCommandPrefix = "EducationPortal";
        public static string UserPrefix = "User: ";
        public static string CoursePrefix = "Course: ";

        public static string ErrorUnknownCommand = "\nНеизвестная команда: \"{0}\". Вызовите команду \"help\" для вывода списка доступных команд\n";
        public static string ErrorWrongParamsCount = "Указано неверное число параметров!";
        public static string ErrorWrongParamValue = "Указано недопустимое значение параметра(-ов)";
        public static string ErrorTryRegWhileLoggedIn = "Для регистрации пользователя необходимо выйти из учетной записи!";
        public static string ErrorTryLogInWhileLoggedIn = "Вы уже вошли в систему!";
        public static string ErrorTryLogOutWhileLoggedOut = "Вы еще не вошли в систему!";
        public static string ErrorTryCommandWhileLoggedOut = "Для выполнения этой команды необходимо авторизоваться!";
        public static string ErrorIncorrectNumberOfCourse = "Введен некорректный номер курса!";
        public static string ErrorIncorrectNumberOfMaterial = "Введен некорректный номер материала!";
        public static string ErrorCourseListIsEmpty = "Сперва получите список курсов!";
        public static string ErrorNoSelectedCourse = "Вы не выбрали курс!";
        public static string ErrorAlreadyInCourseMode = "Сперва покиньте выбранный курс!";
        public static string ErrorNotJoiningCourse = "Вы не являетесь участником данного курса!";
        public static string ErrorAlreadyCompleteCourse = "Вы уже прошли данный курс!";

        public static string InfoLoggedOut = "Вы покинули учетную запись";

        public static string InputCourseName = "Введите название курса: ";
        public static string InputCourseDescription = "Введите описание курса: ";
        public static string InputMaterialName = "Введите название нового материала: ";
        public static string InputMaterialUrl= "Введите URL нового материала: ";

        public static string OutputCourseName = "Название курса: {0}";
        public static string OutputCourseAuthorName = "Автор курса: {0}";
        public static string OutputCourseDescription = "Описание: {0}";
        public static string OutputCourseSkills = "По прохождению курса можно получить следующие навыки: {0}";
        public static string OutputEmptySkillList = "(Пусто)";
        public static string OutputIsCourseAuthor = "Вы - автор данного курса";
        public static string OutputIsJoiningCourse = "Вы участвуете в данном курсе";
        public static string OutputHasCompleteCourse = "Вы уже прошли данный курс";
        
        


    }
}
