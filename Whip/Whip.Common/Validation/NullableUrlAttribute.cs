using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;

namespace Whip.Common
{
    public class NullableUrlAttribute : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (value == null)
                return ValidationResult.Success;

            var url = value.ToString();

            if (string.IsNullOrEmpty(url))
                return ValidationResult.Success;

            if (Regex.IsMatch(url, Validation.UrlRegexPattern))
                return ValidationResult.Success;

            return new ValidationResult(string.Format("The value entered for {0} is not a valid URL", validationContext.DisplayName));
        }
    }
}
