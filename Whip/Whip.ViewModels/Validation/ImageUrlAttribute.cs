using System.ComponentModel.DataAnnotations;
using Whip.Common;
using Whip.Common.Validation;

namespace Whip.ViewModels.Validation
{
    public class ImageUrlAttribute : ValidationAttribute
    {
        public ImageUrlAttribute(params ImageType[] types)
        {
            Types = types;
        }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (value == null)
                return ValidationResult.Success;

            var url = value.ToString();

            if (UrlValidation.IsValidUrl(url) && ImageValidation.HasValidImageExtension(url, ImageType.Ico, ImageType.Jpeg, ImageType.Png))
                return ValidationResult.Success;

            return new ValidationResult(string.Format("The value entered for {0} is not a valid image URL", validationContext.DisplayName));
        }

        public ImageType[] Types { get; set; }
    }
}
