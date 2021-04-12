namespace EducationPortal.ConsoleUI.Commands
{
    using System;
    using System.Linq;
    using EducationPortal.BLL.DTO;
    using EducationPortal.BLL.Services;
    using EducationPortal.BLL.Validation;
    using EducationPortal.ConsoleUI.Resources;
    using EducationPortal.ConsoleUI.Utilities;
    using FluentValidation.Results;

    public class AddMaterialCommand : ICommand
    {
        private ClientData client;
        private ICourseService courseService;
        private IMaterialService materialService;
        private IValidator<MaterialDTO> materialValidator;

        public AddMaterialCommand(ICourseService courseService, IMaterialService materialService, IValidator<MaterialDTO> materialValidator, ClientData client)
        {
            this.client = client;
            this.materialService = materialService;
            this.courseService = courseService;
            this.materialValidator = materialValidator;
        }

        public string Name => "addmaterial";

        public string Description => "addmaterial [-new | -exist]\nДобавить материал к выбранному курсу\n";

        public int ParamsCount => 1;

        public void Execute()
        {
            if (!this.client.IsAuthorized)
            {
                Console.WriteLine(ConsoleMessages.ErrorTryCommandWhileLoggedOut);
                return;
            }

            if (this.client.SelectedCourse == null)
            {
                Console.WriteLine(ConsoleMessages.ErrorNoSelectedCourse);
                return;
            }

            var checkResponse = this.courseService.CanEditCourse(this.client.Id, this.client.SelectedCourse.Id);

            if (!checkResponse.IsSuccessful)
            {
                Console.WriteLine(ResourceHelper.GetMessageString(checkResponse.MessageCode));
                return;
            }

            if (this.client.InputBuffer[0] == "-new")
            {
                Console.WriteLine(ConsoleMessages.InputMaterialName);
                var name = Console.ReadLine();

                Console.WriteLine(ConsoleMessages.InputMaterialUrl);
                var url = Console.ReadLine();

                Console.WriteLine(ConsoleMessages.InputMaterialType);
                var materialType = Console.ReadLine();

                MaterialDTO materialToAdd;
                ValidationResult validationResult;

                switch (materialType)
                {
                    case "Статья":

                        Console.WriteLine(ConsoleMessages.InputPublicationDate);
                        var date = Console.ReadLine();

                        materialToAdd = new ArticleDTO()
                        {
                            Name = name,
                            Url = url,
                            PublicationDate = date,
                        };

                        validationResult = this.materialValidator.Validate(materialToAdd, "Common", "Article");

                        break;

                    case "Книга":

                        Console.WriteLine(ConsoleMessages.InputAuthorNames);
                        var authors = Console.ReadLine();

                        Console.WriteLine(ConsoleMessages.InputPagesCount);
                        var pageCount = Console.ReadLine();

                        Console.WriteLine(ConsoleMessages.InputFormat);
                        var format = Console.ReadLine().Trim();

                        Console.WriteLine(ConsoleMessages.InputPublicationYear);
                        var year = Console.ReadLine();

                        materialToAdd = new BookDTO()
                        {
                            Name = name,
                            Url = url,
                            Format = format,
                            AuthorNames = authors,
                            PageCount = pageCount,
                            PublishingYear = year,
                        };

                        validationResult = this.materialValidator.Validate(materialToAdd, "Common", "Book");

                        break;

                    case "Видео":

                        Console.WriteLine(ConsoleMessages.InputDuration);
                        var duration = Console.ReadLine();

                        Console.WriteLine(ConsoleMessages.InputQuality);
                        var quality = Console.ReadLine().Trim();

                        materialToAdd = new VideoDTO()
                        {
                            Name = name,
                            Url = url,
                            Duration = duration,
                            Quality = quality,
                        };

                        validationResult = this.materialValidator.Validate(materialToAdd, "Common", "Video");

                        break;

                    default:
                        Console.WriteLine(ConsoleMessages.ErrorWrongMaterialType);
                        return;
                }

                if (!validationResult.IsValid)
                {
                    var errorCode = validationResult.Errors[0].ErrorCode;
                    OutputHelper.PrintValidationError(errorCode);
                    return;
                }

                var materialResponse = this.materialService.AddMaterial(materialToAdd);

                if (!materialResponse.IsSuccessful)
                {
                    Console.WriteLine(ResourceHelper.GetMessageString(materialResponse.MessageCode));
                    return;
                }

                materialToAdd.Id = materialResponse.MaterialId;

                var addMaterialResponse = this.courseService.AddMaterialToCourse(this.client.Id, this.client.SelectedCourse.Id, materialToAdd.Id);
                Console.WriteLine(ResourceHelper.GetMessageString(addMaterialResponse.MessageCode));
            }
            else if (this.client.InputBuffer[0] == "-exist")
            {
                var response = this.materialService.GetAllMaterials();

                if (!response.IsSuccessful)
                {
                    Console.WriteLine(ResourceHelper.GetMessageString(response.MessageCode));
                    return;
                }

                var allMaterials = response.Materials.ToArray();
                OutputHelper.PrintMaterials(allMaterials);

                Console.WriteLine(ConsoleMessages.InputMaterialNumber);
                var numberStr = Console.ReadLine();

                var isNumberCorrect = long.TryParse(numberStr, out long number);

                if (!isNumberCorrect
                 || number - 1 < 0
                 || number - 1 >= allMaterials.Length)
                {
                    Console.WriteLine(ConsoleMessages.ErrorIncorrectNumberOfMaterial);
                    return;
                }

                var checkMaterialResponse = this.materialService.CheckMaterialExisting(allMaterials[number - 1].Id);

                if (!checkMaterialResponse.IsSuccessful)
                {
                    Console.WriteLine(ResourceHelper.GetMessageString(checkMaterialResponse.MessageCode));
                    return;
                }

                var addMaterialResponse = this.courseService.AddMaterialToCourse(this.client.Id, this.client.SelectedCourse.Id, allMaterials[number - 1].Id);
                Console.WriteLine(ResourceHelper.GetMessageString(addMaterialResponse.MessageCode));
            }
            else
            {
                Console.WriteLine(ConsoleMessages.ErrorWrongParamValue);
            }
        }
    }
}
