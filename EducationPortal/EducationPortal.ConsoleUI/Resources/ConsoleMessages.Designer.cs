﻿//------------------------------------------------------------------------------
// <auto-generated>
//     Этот код создан программой.
//     Исполняемая версия:4.0.30319.42000
//
//     Изменения в этом файле могут привести к неправильной работе и будут потеряны в случае
//     повторной генерации кода.
// </auto-generated>
//------------------------------------------------------------------------------

namespace EducationPortal.ConsoleUI.Resources {
    using System;
    
    
    /// <summary>
    ///   Класс ресурса со строгой типизацией для поиска локализованных строк и т.д.
    /// </summary>
    // Этот класс создан автоматически классом StronglyTypedResourceBuilder
    // с помощью такого средства, как ResGen или Visual Studio.
    // Чтобы добавить или удалить член, измените файл .ResX и снова запустите ResGen
    // с параметром /str или перестройте свой проект VS.
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Resources.Tools.StronglyTypedResourceBuilder", "16.0.0.0")]
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
    [global::System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    public class ConsoleMessages {
        
        private static global::System.Resources.ResourceManager resourceMan;
        
        private static global::System.Globalization.CultureInfo resourceCulture;
        
        [global::System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        internal ConsoleMessages() {
        }
        
        /// <summary>
        ///   Возвращает кэшированный экземпляр ResourceManager, использованный этим классом.
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        public static global::System.Resources.ResourceManager ResourceManager {
            get {
                if (object.ReferenceEquals(resourceMan, null)) {
                    global::System.Resources.ResourceManager temp = new global::System.Resources.ResourceManager("EducationPortal.ConsoleUI.Resources.ConsoleMessages", typeof(ConsoleMessages).Assembly);
                    resourceMan = temp;
                }
                return resourceMan;
            }
        }
        
        /// <summary>
        ///   Перезаписывает свойство CurrentUICulture текущего потока для всех
        ///   обращений к ресурсу с помощью этого класса ресурса со строгой типизацией.
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        public static global::System.Globalization.CultureInfo Culture {
            get {
                return resourceCulture;
            }
            set {
                resourceCulture = value;
            }
        }
        
        /// <summary>
        ///   Ищет локализованную строку, похожую на Course: .
        /// </summary>
        public static string CoursePrefix {
            get {
                return ResourceManager.GetString("CoursePrefix", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Ищет локализованную строку, похожую на EducationPortal.
        /// </summary>
        public static string DefaultCommandPrefix {
            get {
                return ResourceManager.GetString("DefaultCommandPrefix", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Ищет локализованную строку, похожую на Вы уже прошли данный курс!.
        /// </summary>
        public static string ErrorAlreadyCompleteCourse {
            get {
                return ResourceManager.GetString("ErrorAlreadyCompleteCourse", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Ищет локализованную строку, похожую на Сперва покиньте выбранный курс!.
        /// </summary>
        public static string ErrorAlreadyInCourseMode {
            get {
                return ResourceManager.GetString("ErrorAlreadyInCourseMode", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Ищет локализованную строку, похожую на Сперва получите список курсов!.
        /// </summary>
        public static string ErrorCourseListIsEmpty {
            get {
                return ResourceManager.GetString("ErrorCourseListIsEmpty", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Ищет локализованную строку, похожую на Введен некорректный номер курса!.
        /// </summary>
        public static string ErrorIncorrectNumberOfCourse {
            get {
                return ResourceManager.GetString("ErrorIncorrectNumberOfCourse", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Ищет локализованную строку, похожую на Введен некорректный номер материала!.
        /// </summary>
        public static string ErrorIncorrectNumberOfMaterial {
            get {
                return ResourceManager.GetString("ErrorIncorrectNumberOfMaterial", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Ищет локализованную строку, похожую на Вы не выбрали курс!.
        /// </summary>
        public static string ErrorNoSelectedCourse {
            get {
                return ResourceManager.GetString("ErrorNoSelectedCourse", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Ищет локализованную строку, похожую на Вы не являетесь участником данного курса!.
        /// </summary>
        public static string ErrorNotJoiningCourse {
            get {
                return ResourceManager.GetString("ErrorNotJoiningCourse", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Ищет локализованную строку, похожую на Для выполнения этой команды необходимо авторизоваться!.
        /// </summary>
        public static string ErrorTryCommandWhileLoggedOut {
            get {
                return ResourceManager.GetString("ErrorTryCommandWhileLoggedOut", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Ищет локализованную строку, похожую на Вы уже вошли в систему!.
        /// </summary>
        public static string ErrorTryLogInWhileLoggedIn {
            get {
                return ResourceManager.GetString("ErrorTryLogInWhileLoggedIn", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Ищет локализованную строку, похожую на Вы еще не вошли в систему!.
        /// </summary>
        public static string ErrorTryLogOutWhileLoggedOut {
            get {
                return ResourceManager.GetString("ErrorTryLogOutWhileLoggedOut", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Ищет локализованную строку, похожую на Для регистрации пользователя необходимо выйти из учетной записи!.
        /// </summary>
        public static string ErrorTryRegWhileLoggedIn {
            get {
                return ResourceManager.GetString("ErrorTryRegWhileLoggedIn", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Ищет локализованную строку, похожую на 
        ///Неизвестная команда: &quot;{0}&quot;. Вызовите команду &quot;help&quot; для вывода списка доступных команд
        ///.
        /// </summary>
        public static string ErrorUnknownCommand {
            get {
                return ResourceManager.GetString("ErrorUnknownCommand", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Ищет локализованную строку, похожую на Неверно указан тип материала!.
        /// </summary>
        public static string ErrorWrongMaterialType {
            get {
                return ResourceManager.GetString("ErrorWrongMaterialType", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Ищет локализованную строку, похожую на Указано неверное число параметров!.
        /// </summary>
        public static string ErrorWrongParamsCount {
            get {
                return ResourceManager.GetString("ErrorWrongParamsCount", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Ищет локализованную строку, похожую на Указано недопустимое значение параметра(-ов).
        /// </summary>
        public static string ErrorWrongParamValue {
            get {
                return ResourceManager.GetString("ErrorWrongParamValue", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Ищет локализованную строку, похожую на Дата издания: {0}.
        /// </summary>
        public static string InfoArticlePublicationDate {
            get {
                return ResourceManager.GetString("InfoArticlePublicationDate", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Ищет локализованную строку, похожую на Авторы: {0}.
        /// </summary>
        public static string InfoBookAuthorNames {
            get {
                return ResourceManager.GetString("InfoBookAuthorNames", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Ищет локализованную строку, похожую на Формат: {0}.
        /// </summary>
        public static string InfoBookFormat {
            get {
                return ResourceManager.GetString("InfoBookFormat", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Ищет локализованную строку, похожую на Год издания: {0}.
        /// </summary>
        public static string InfoBookPublishingYear {
            get {
                return ResourceManager.GetString("InfoBookPublishingYear", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Ищет локализованную строку, похожую на Вы покинули учетную запись.
        /// </summary>
        public static string InfoLoggedOut {
            get {
                return ResourceManager.GetString("InfoLoggedOut", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Ищет локализованную строку, похожую на Название: {0}.
        /// </summary>
        public static string InfoMaterialName {
            get {
                return ResourceManager.GetString("InfoMaterialName", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Ищет локализованную строку, похожую на URL: {0}.
        /// </summary>
        public static string InfoMaterialUrl {
            get {
                return ResourceManager.GetString("InfoMaterialUrl", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Ищет локализованную строку, похожую на Продолжительность: {0}.
        /// </summary>
        public static string InfoVideoDuration {
            get {
                return ResourceManager.GetString("InfoVideoDuration", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Ищет локализованную строку, похожую на {0:d2}:{1:d2}.
        /// </summary>
        public static string InfoVideoDurationTemplate {
            get {
                return ResourceManager.GetString("InfoVideoDurationTemplate", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Ищет локализованную строку, похожую на Качество: {0}.
        /// </summary>
        public static string InfoVideoQuality {
            get {
                return ResourceManager.GetString("InfoVideoQuality", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Ищет локализованную строку, похожую на Введите имена авторов, через запятую: .
        /// </summary>
        public static string InputAuthorNames {
            get {
                return ResourceManager.GetString("InputAuthorNames", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Ищет локализованную строку, похожую на Введите описание курса: .
        /// </summary>
        public static string InputCourseDescription {
            get {
                return ResourceManager.GetString("InputCourseDescription", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Ищет локализованную строку, похожую на Введите название курса: .
        /// </summary>
        public static string InputCourseName {
            get {
                return ResourceManager.GetString("InputCourseName", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Ищет локализованную строку, похожую на Введите длительность видео(в секундах): .
        /// </summary>
        public static string InputDuration {
            get {
                return ResourceManager.GetString("InputDuration", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Ищет локализованную строку, похожую на Введите формат книги: .
        /// </summary>
        public static string InputFormat {
            get {
                return ResourceManager.GetString("InputFormat", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Ищет локализованную строку, похожую на Введите название нового материала: .
        /// </summary>
        public static string InputMaterialName {
            get {
                return ResourceManager.GetString("InputMaterialName", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Ищет локализованную строку, похожую на 
        ///Введите номер материала:.
        /// </summary>
        public static string InputMaterialNumber {
            get {
                return ResourceManager.GetString("InputMaterialNumber", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Ищет локализованную строку, похожую на Введите тип материала (Статья | Книга | Видео).
        /// </summary>
        public static string InputMaterialType {
            get {
                return ResourceManager.GetString("InputMaterialType", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Ищет локализованную строку, похожую на Введите URL нового материала: .
        /// </summary>
        public static string InputMaterialUrl {
            get {
                return ResourceManager.GetString("InputMaterialUrl", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Ищет локализованную строку, похожую на Введите количество страниц: .
        /// </summary>
        public static string InputPagesCount {
            get {
                return ResourceManager.GetString("InputPagesCount", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Ищет локализованную строку, похожую на Введите дату публикации статьи (ГГГГ-ММ-ДД): .
        /// </summary>
        public static string InputPublicationDate {
            get {
                return ResourceManager.GetString("InputPublicationDate", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Ищет локализованную строку, похожую на Введите год издания книги: .
        /// </summary>
        public static string InputPublicationYear {
            get {
                return ResourceManager.GetString("InputPublicationYear", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Ищет локализованную строку, похожую на Введите качество видео: .
        /// </summary>
        public static string InputQuality {
            get {
                return ResourceManager.GetString("InputQuality", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Ищет локализованную строку, похожую на Автор курса: {0}.
        /// </summary>
        public static string OutputCourseAuthorName {
            get {
                return ResourceManager.GetString("OutputCourseAuthorName", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Ищет локализованную строку, похожую на Курс {0} успешно завершен!.
        /// </summary>
        public static string OutputCourseCompleted {
            get {
                return ResourceManager.GetString("OutputCourseCompleted", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Ищет локализованную строку, похожую на Описание: {0}.
        /// </summary>
        public static string OutputCourseDescription {
            get {
                return ResourceManager.GetString("OutputCourseDescription", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Ищет локализованную строку, похожую на Название курса: {0}.
        /// </summary>
        public static string OutputCourseName {
            get {
                return ResourceManager.GetString("OutputCourseName", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Ищет локализованную строку, похожую на По прохождению курса можно получить следующие навыки: {0}.
        /// </summary>
        public static string OutputCourseSkills {
            get {
                return ResourceManager.GetString("OutputCourseSkills", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Ищет локализованную строку, похожую на {0}. {1}
        ///{2}
        ///Умения: {3}
        ///.
        /// </summary>
        public static string OutputCourseTemplate {
            get {
                return ResourceManager.GetString("OutputCourseTemplate", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Ищет локализованную строку, похожую на (Пусто).
        /// </summary>
        public static string OutputEmptySkillList {
            get {
                return ResourceManager.GetString("OutputEmptySkillList", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Ищет локализованную строку, похожую на Вы уже прошли данный курс.
        /// </summary>
        public static string OutputHasCompleteCourse {
            get {
                return ResourceManager.GetString("OutputHasCompleteCourse", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Ищет локализованную строку, похожую на Вы - автор данного курса.
        /// </summary>
        public static string OutputIsCourseAuthor {
            get {
                return ResourceManager.GetString("OutputIsCourseAuthor", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Ищет локализованную строку, похожую на Вы участвуете в данном курсе.
        /// </summary>
        public static string OutputIsJoiningCourse {
            get {
                return ResourceManager.GetString("OutputIsJoiningCourse", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Ищет локализованную строку, похожую на {0}. {1}
        ///.
        /// </summary>
        public static string OutputMaterialTemplate {
            get {
                return ResourceManager.GetString("OutputMaterialTemplate", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Ищет локализованную строку, похожую на Изучен новый материал!.
        /// </summary>
        public static string OutputNewMaterialLearned {
            get {
                return ResourceManager.GetString("OutputNewMaterialLearned", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Ищет локализованную строку, похожую на Получен новый навык - &quot;{0}&quot;.
        /// </summary>
        public static string OutputNewSkillRecieved {
            get {
                return ResourceManager.GetString("OutputNewSkillRecieved", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Ищет локализованную строку, похожую на Завершенные курсы: {0}.
        /// </summary>
        public static string OutputProfileCompletedCourses {
            get {
                return ResourceManager.GetString("OutputProfileCompletedCourses", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Ищет локализованную строку, похожую на Курсы в процессе: {0}.
        /// </summary>
        public static string OutputProfileJoinedCourses {
            get {
                return ResourceManager.GetString("OutputProfileJoinedCourses", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Ищет локализованную строку, похожую на Навык: {0}.
        /// </summary>
        public static string OutputProfileSkills {
            get {
                return ResourceManager.GetString("OutputProfileSkills", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Ищет локализованную строку, похожую на Профиль пользователя {0}.
        /// </summary>
        public static string OutputProfileUserName {
            get {
                return ResourceManager.GetString("OutputProfileUserName", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Ищет локализованную строку, похожую на Навык &quot;{0}&quot; повышен!.
        /// </summary>
        public static string OutputSkillIncreased {
            get {
                return ResourceManager.GetString("OutputSkillIncreased", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Ищет локализованную строку, похожую на User: .
        /// </summary>
        public static string UserPrefix {
            get {
                return ResourceManager.GetString("UserPrefix", resourceCulture);
            }
        }
    }
}
