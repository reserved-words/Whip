using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;
using Whip.Common.Validation;

namespace Whip.ViewModels.Validation
{
    public class PartialUrlStringAttribute : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var str = value.ToString();

            if (str.Length > UrlValidation.PartialUrlStringMaxLength)
            {
                return new ValidationResult(string.Format("The term entered for {0} should not be more than {1} characters", validationContext.DisplayName, UrlValidation.PartialUrlStringMaxLength));
            }

            if (!Regex.IsMatch(str, UrlValidation.PartialUrlStringRegexPattern))
            {
                return new ValidationResult(string.Format("The term entered for {0} is not a valid part of a URL", validationContext.DisplayName));
            }

            return ValidationResult.Success;
        }
    }
}
