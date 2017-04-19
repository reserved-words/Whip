using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;
using Whip.Common.Validation;

namespace Whip.ViewModels.Validation
{
    public class FacebookUsernameAttribute : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var username = value.ToString();

            if (username.Length > SocialMediaValidation.FacebookUsernameMaxLength)
            {
                return new ValidationResult($"A Facebook username can only contain up to {SocialMediaValidation.FacebookUsernameMaxLength} characters");
            }

            if (!Regex.IsMatch(username, SocialMediaValidation.FacebookUsernameRegexPattern))
            {
                return new ValidationResult("The Facebook username entered contains invalid characters");
            }

            return ValidationResult.Success;
        }
    }
}
