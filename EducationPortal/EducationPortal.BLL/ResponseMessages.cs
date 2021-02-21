namespace EducationPortal.BLL
{
    public static class ResponseMessages
    {
        #region CourseServiceMessages
        public static string CourseNotFound = "Указанного курса не существует!";
        public static string CourseAlreadyCompleted = "Вы уже прошли данный курс!";

        public static string AddCourseSuccess = "Новый курс успешно создан!";

        public static string EditCourseSuccess = "Курс успешно обновлен";

        public static string AddSkillAlreadyExists = "Курс уже содержит такое умение!";
        public static string AddSkillSuccess = "Умение успешно добавлено!";

        public static string RemoveSkillNotFound = "У выбранного курса нет указанного умения!";
        public static string RemoveSkillSuccess = "Умение успешно удалено!";

        public static string AddMaterialToCourseAlreadyExists = "Данный курс уже содержит этот материал!";
        public static string AddMaterialToCourseSuccess = "Материал успешно добавлен к курсу!";

        public static string CanEditCourseNotAnAuthor = "Вы не являетесь автором данного курса";

        public static string CanJoinCourseAlreadyJoin = "Вы уже участник данного курса!";
        #endregion

        #region MaterialServiceMessages
        public static string GetAllMaterialsEmptyResult = "Список материалов пуст!";

        public static string CheckMaterialExistingNotFound = "Данного материала не существует!";
        #endregion

        #region UserServiceMessages
        public static string UserNotFound = "Указанного пользователя не существует!";
        public static string CourseNotJoined = "Вы не участвуете в этом курсе!";

        public static string AuthorizeWrongCredentials = "Неверно введенное имя пользователя, email или пароль!";

        public static string RegisterEmailUsed = "Данный Email уже занят!";
        public static string RegisterLoginUsed = "Данный логин уже занят!";
        public static string RegisterSuccess = "Новый пользователь зарегистрирован!";

        public static string GetUserByIdNotFound = "Не удалось найти информацию о пользователе!";

        public static string JoinToCourseSuccess = "Начато изучение нового курса!";

        public static string AddCompletedCourseNotCompleted = "Изучены не все материалы курса!";

        public static string GetNextMaterialAnyNewMaterial = "Все материалы курса пройдены!";
        #endregion
    }
}
