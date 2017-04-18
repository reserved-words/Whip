using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;

namespace Whip.Common
{
    public class PartialUrlStringAttribute : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var str = value.ToString();

            if (str.Length > Validation.PartialUrlStringMaxLength)
            {
                return new ValidationResult(string.Format("The term entered for {0} should not be more than {1} characters", validationContext.DisplayName, Validation.PartialUrlStringMaxLength));
            }

            if (!Regex.IsMatch(str, Validation.PartialUrlStringRegexPattern))
            {
                return new ValidationResult(string.Format("The term entered for {0} is not a valid part of a URL", validationContext.DisplayName));
            }

            return ValidationResult.Success;
        }
    }
}
