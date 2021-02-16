namespace EducationPortal.ConsoleUI.Validation
{
    using System;
    using System.IO;
    using EducationPortal.BLL;
    using EducationPortal.BLL.DTO;

    public class MaterialDataValidator
    {
        private MaterialDTO material;
        private Uri uri;

        public MaterialDataValidator(MaterialDTO material)
        {
            this.material = material;
        }

        public ValidationResult Validate()
        {
            if (this.material.Name.Length < DataSettings.MaterialNameMinCharacterCount
             || this.material.Name.Length > DataSettings.MaterialNameMaxCharacterCount)
            {
                return new ValidationResult()
                {
                    IsValid = false,
                    Message = string.Format(
                        "Название материала должно быть длиной от {0} до {1} символов!",
                        DataSettings.MaterialNameMinCharacterCount,
                        DataSettings.MaterialNameMaxCharacterCount),
                };
            }

            var isUrlCorrect = Uri.TryCreate(this.material.Url, UriKind.Absolute, out this.uri)
                            && (this.uri.Scheme == Uri.UriSchemeHttp || this.uri.Scheme == Uri.UriSchemeHttps);

            if (!isUrlCorrect)
            {
                return new ValidationResult()
                {
                    IsValid = false,
                    Message = "Некорректный формат URL!",
                };
            }

            if (this.material is BookDTO)
            {
                return this.ValidateAsBook();
            }
            else if (this.material is ArticleDTO)
            {
                return this.ValidateAsArticle();
            }
            else if (this.material is VideoDTO)
            {
                return this.ValidateAsVideo();
            }

            return new ValidationResult()
            {
                IsValid = false,
                Message = "Неопознанный тип материала!",
            };
        }

        private ValidationResult ValidateAsVideo()
        {
            var video = this.material as VideoDTO;

            if (!int.TryParse(video.Duration, out int value)
            || value <= 0)
            {
                return new ValidationResult()
                {
                    IsValid = false,
                    Message = "Некорректное значение длительности видео!",
                };
            }

            return new ValidationResult()
            {
                IsValid = true,
                Message = string.Empty,
            };
        }

        private ValidationResult ValidateAsArticle()
        {
            var article = this.material as ArticleDTO;
            if (!DateTime.TryParse(article.PublicationDate, out _))
            {
                return new ValidationResult()
                {
                    IsValid = false,
                    Message = "Некорректный формат даты!",
                };
            }

            return new ValidationResult()
            {
                IsValid = true,
                Message = string.Empty,
            };
        }

        private ValidationResult ValidateAsBook()
        {
            var book = this.material as BookDTO;

            if (!Path.HasExtension(this.uri.AbsolutePath))
            {
                return new ValidationResult()
                {
                    IsValid = false,
                    Message = "URL книги должен ссылаться на файл!",
                };
            }

            if (!int.TryParse(book.PageCount, out int value)
             || value <= 0)
            {
                return new ValidationResult()
                {
                    IsValid = false,
                    Message = "Некорректное значение количества страниц!",
                };
            }

            if (!int.TryParse(book.PublishingYear, out value)
             || value < 0)
            {
                return new ValidationResult()
                {
                    IsValid = false,
                    Message = "Некорректное значение года издания!",
                };
            }

            return new ValidationResult()
            {
                IsValid = true,
                Message = string.Empty,
            };
        }
    }
}
