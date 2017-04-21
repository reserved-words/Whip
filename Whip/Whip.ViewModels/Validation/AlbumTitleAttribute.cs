using System.ComponentModel.DataAnnotations;
using System.Linq;
using Whip.Common.Validation;
using Whip.ViewModels.TabViewModels.EditTrack;

namespace Whip.ViewModels.Validation
{
    public class AlbumTitleAttribute : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (value == null)
                return new ValidationResult(string.Format("{0} is required", validationContext.DisplayName));

            var title = value.ToString();

            if (title == string.Empty)
                return new ValidationResult(string.Format("{0} is required", validationContext.DisplayName));

            if (title.Length > TrackValidation.MaxLengthAlbumTitle)
            {
                return new ValidationResult(string.Format("{0} must not be longer than {1} characters", validationContext.DisplayName, TrackValidation.MaxLengthAlbumTitle));
            }

            var viewModel = validationContext.ObjectInstance as AlbumViewModel;

            var existingAlbums = viewModel.Albums;

            var selectedAlbum = viewModel.Album;

            if (selectedAlbum.Title != title && existingAlbums.Any(a => a.Title == title))
            {
                return new ValidationResult(string.Format("The title entered for {0} already exists for this artist - please select the existing album from the dropdown list", validationContext.DisplayName));
            }

            return ValidationResult.Success;
        }
    }
}
