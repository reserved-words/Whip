using System.ComponentModel.DataAnnotations;
using Whip.Common.Validation;

namespace Whip.ViewModels.Validation
{
    public class TrackCountAttribute : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            int trackCount;

            if (!int.TryParse(value.ToString(), out trackCount))
            {
                return new ValidationResult(GetNumberRangeErrorMessage(validationContext));
            }

            if (trackCount < TrackValidation.MinTrackCount || trackCount > TrackValidation.MaxTrackCount)
            {
                return new ValidationResult(GetNumberRangeErrorMessage(validationContext));
            }

            return ValidationResult.Success;
        }

        private string GetNumberRangeErrorMessage(ValidationContext context)
        {
            return string.Format("{0} must be a number between {1} and {2}",
                context.DisplayName,
                TrackValidation.MinTrackCount,
                TrackValidation.MaxTrackCount);
        }
    }
}
