namespace EducationPortal.ConsoleUI
{
    public static class ConsoleMessages
    {
        public static string DefaultCommandPrefix = "EducationPortal";

        public static string ErrorUnknownCommand = "\nНеизвестная команда: \"{0}\". Вызовите команду \"help\" для вывода списка доступных команд\n";
        public static string ErrorWrongParamsCount = "Указано неверное число параметров!";
        public static string ErrorTryRegWhileLoggedIn = "Для регистрации пользователя необходимо выйти из учетной записи!";
        public static string ErrorTryLogInWhileLoggedIn = "Вы уже вошли в систему!";
        public static string ErrorTryLogOutWhileLoggedOut = "Вы еще не вошли в систему!";

        public static string InfoLoggedOut = "Вы покинули учетную запись";


    }
}
