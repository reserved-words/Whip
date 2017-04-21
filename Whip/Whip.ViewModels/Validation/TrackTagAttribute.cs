using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;
using Whip.Common.Validation;
using Whip.ViewModels.TabViewModels.EditTrack;

namespace Whip.ViewModels.Validation
{
    public class TrackTagAttribute : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (value == null)
                return ValidationResult.Success;

            var tag = value.ToString();

            var errorMessage = GetErrorMessage(tag, validationContext.ObjectInstance);

            return string.IsNullOrEmpty(errorMessage)
                ? ValidationResult.Success
                : new ValidationResult(errorMessage);
        }

        private static string GetErrorMessage(string tag, object trackViewModel)
        {
            if (tag.Length > TrackValidation.MaxLengthTag)
            {
                return string.Format("Tags must not be longer than {0} characters", TrackValidation.MaxLengthTag);
            }

            if (!Regex.IsMatch(tag, TrackValidation.TrackTagRegexPattern))
            {
                return TrackValidation.TrackTagCharactersErrorMessage;
            }

            var track = trackViewModel as TrackViewModel;

            if (track.Tags.Contains(tag))
            {
                return "The entered tag already belongs to this track";
            }

            return null;
        }

        public static bool Validate(string tag, object trackViewModel)
        {
            return string.IsNullOrEmpty(GetErrorMessage(tag, trackViewModel));
        }
    }
}
