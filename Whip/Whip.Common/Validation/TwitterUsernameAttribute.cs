using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;

namespace Whip.Common
{
    public class TwitterUsernameAttribute : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var username = value.ToString();

            if (username.Length > Validation.TwitterUsernameMaxLength)
            {
                return new ValidationResult($"A Twitter username can only contain up to {Validation.TwitterUsernameMaxLength} characters");
            }

            if (!Regex.IsMatch(username, Validation.TwitterUsernameRegexPattern))
            {
                return new ValidationResult("The Twitter username entered contains invalid characters");
            }

            return ValidationResult.Success;
        }
    }
}
