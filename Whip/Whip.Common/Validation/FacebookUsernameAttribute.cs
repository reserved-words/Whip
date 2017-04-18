using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;

namespace Whip.Common
{
    public class FacebookUsernameAttribute : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var username = value.ToString();

            if (username.Length > Validation.FacebookUsernameMaxLength)
            {
                return new ValidationResult($"A Facebook username can only contain up to {Validation.FacebookUsernameMaxLength} characters");
            }

            if (!Regex.IsMatch(username, Validation.FacebookUsernameRegexPattern))
            {
                return new ValidationResult("The Facebook username entered contains invalid characters");
            }

            return ValidationResult.Success;
        }
    }
}
