using System.ComponentModel.DataAnnotations;
using Whip.Common.Validation;

namespace Whip.ViewModels.Validation
{
    public class DiscCountAttribute : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            int discCount;

            if (!int.TryParse(value.ToString(), out discCount))
            {
                return new ValidationResult(GetNumberRangeErrorMessage(validationContext));
            }

            if (discCount < TrackValidation.MinDiscCount || discCount > TrackValidation.MaxDiscCount)
            {
                return new ValidationResult(GetNumberRangeErrorMessage(validationContext));
            }

            return ValidationResult.Success;
        }

        private string GetNumberRangeErrorMessage(ValidationContext context)
        {
            return string.Format("{0} must be a number between {1} and {2}",
                context.DisplayName,
                TrackValidation.MinDiscCount,
                TrackValidation.MaxDiscCount);
        }
    }
}
