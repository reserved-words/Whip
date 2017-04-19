using System.ComponentModel.DataAnnotations;

namespace Whip.ViewModels.Validation
{
    public class YearAttribute : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var str = value.ToString();

            if (str.Length != 4)
            {
                return new ValidationResult(string.Format("{0} must be a four digit number", validationContext.DisplayName));
            }

            int year;

            if (!int.TryParse(str, out year))
            {
                return new ValidationResult(string.Format("{0} must be a four digit number", validationContext.DisplayName));
            }

            return ValidationResult.Success;
        }
    }
}
