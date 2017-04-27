using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;
using Whip.Common.Validation;

namespace Whip.ViewModels.Validation
{
    public class YouTubeUsername : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var username = value.ToString();

            if (username.Length > SocialMediaValidation.YouTubeUsernameMaxLength)
            {
                return new ValidationResult($"A YouTube username can only contain up to {SocialMediaValidation.YouTubeUsernameMaxLength} characters");
            }

            if (!Regex.IsMatch(username, SocialMediaValidation.YouTubeUsernameRegexPattern))
            {
                return new ValidationResult("The YouTube username entered contains invalid characters");
            }

            return ValidationResult.Success;
        }
    }
}
