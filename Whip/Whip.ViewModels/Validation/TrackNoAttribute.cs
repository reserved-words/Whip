using System.ComponentModel.DataAnnotations;
using Whip.Common.Validation;
using Whip.ViewModels.TabViewModels;

namespace Whip.ViewModels.Validation
{
    public class TrackNoAttribute : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            int trackNo;

            if (!int.TryParse(value.ToString(), out trackNo))
            {
                return new ValidationResult(GetNumberRangeErrorMessage(validationContext));
            }

            if (trackNo < TrackValidation.MinTrackCount || trackNo > TrackValidation.MaxTrackCount)
            {
                return new ValidationResult(GetNumberRangeErrorMessage(validationContext));
            }

            var track = validationContext.ObjectInstance as TrackViewModel;

            if (trackNo > track.TrackCount)
            {
                return new ValidationResult("Track No must not be greater than the Track Count");
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
