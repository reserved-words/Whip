using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;
using Whip.Common.Validation;

namespace Whip.ViewModels.Validation
{
    public class TwitterUsernameAttribute : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var username = value.ToString();

            if (username.Length > SocialMediaValidation.TwitterUsernameMaxLength)
            {
                return new ValidationResult($"A Twitter username can only contain up to {SocialMediaValidation.TwitterUsernameMaxLength} characters");
            }

            if (!Regex.IsMatch(username, SocialMediaValidation.TwitterUsernameRegexPattern))
            {
                return new ValidationResult("The Twitter username entered contains invalid characters");
            }

            return ValidationResult.Success;
        }
    }
}
