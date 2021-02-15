using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using EducationPortal.BLL;
using EducationPortal.BLL.DTO;

namespace EducationPortal.ConsoleUI.Validation
{
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
            if (material.Name.Length < DataSettings.MaterialNameMinCharacterCount
             || material.Name.Length > DataSettings.MaterialNameMaxCharacterCount)
            {
                return new ValidationResult()
                {
                    IsValid = false,
                    Message = string.Format("Название материала должно быть длиной от {0} до {1} символов!",
                                             DataSettings.MaterialNameMinCharacterCount,
                                             DataSettings.MaterialNameMaxCharacterCount)
                };

            }

            var isUrlCorrect = Uri.TryCreate(material.Url, UriKind.Absolute, out uri)
                            && (uri.Scheme == Uri.UriSchemeHttp || uri.Scheme == Uri.UriSchemeHttps);

            if (!isUrlCorrect)
            {
                return new ValidationResult()
                {
                    IsValid = false,
                    Message = "Некорректный формат URL!"
                };
            }

            if (material is BookDTO)
            {
                return ValidateAsBook();
            }
            else if (material is ArticleDTO)
            {
                return ValidateAsArticle();
            }
            else if (material is VideoDTO)
            {
                return ValidateAsVideo();
            }

            return new ValidationResult()
            {
                IsValid = false,
                Message = "Неопознанный тип материала!"
            };
        }

        private ValidationResult ValidateAsVideo()
        {
            var video = material as VideoDTO;

            if (!int.TryParse(video.Duration, out int value)
            || value <= 0)
            {
                return new ValidationResult()
                {
                    IsValid = false,
                    Message = "Некорректное значение длительности видео!"
                };
            }

            return new ValidationResult()
            {
                IsValid = true,
                Message = string.Empty
            };
        }

        private ValidationResult ValidateAsArticle()
        {
            var article = material as ArticleDTO;

            if (!DateTime.TryParse(article.PublicationDate, out DateTime date))
            {
                return new ValidationResult()
                {
                    IsValid = false,
                    Message = "Некорректный формат даты!"
                };
            }

            return new ValidationResult()
            {
                IsValid = true,
                Message = string.Empty
            };

        }

        private ValidationResult ValidateAsBook()
        {
            var book = material as BookDTO;

            if (!Path.HasExtension(uri.AbsolutePath))
            {
                return new ValidationResult()
                {
                    IsValid = false,
                    Message = "URL книги должен ссылаться на файл!"
                };
            }

            if (!int.TryParse(book.PageCount, out int value)
             || value <= 0)
            {
                return new ValidationResult()
                {
                    IsValid = false,
                    Message = "Некорректное значение количества страниц!"
                };
            }

            if (!int.TryParse(book.PublishingYear, out value)
             || value < 0)
            {
                return new ValidationResult()
                {
                    IsValid = false,
                    Message = "Некорректное значение года издания!"
                };
            }

            return new ValidationResult()
            {
                IsValid = true,
                Message = string.Empty
            };
        }

    }
}
