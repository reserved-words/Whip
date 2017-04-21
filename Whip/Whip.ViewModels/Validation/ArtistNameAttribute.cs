using System.ComponentModel.DataAnnotations;
using System.Linq;
using Whip.Common.Validation;
using Whip.ViewModels.TabViewModels.EditTrack;

namespace Whip.ViewModels.Validation
{
    public class ArtistNameAttribute : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (value == null)
                return new ValidationResult(string.Format("{0} is required", validationContext.DisplayName));

            var name = value.ToString();

            if (name == string.Empty)
                return new ValidationResult(string.Format("{0} is required", validationContext.DisplayName));

            if (name.Length > TrackValidation.MaxLengthArtistName)
            {
                return new ValidationResult(string.Format("{0} must not be longer than {1} characters", validationContext.DisplayName, TrackValidation.MaxLengthArtistName));
            }

            var existingArtists = (validationContext.ObjectInstance as ArtistViewModel)?.Artists
                ?? (validationContext.ObjectInstance as AlbumViewModel)?.Artists;

            var selectedArtist = (validationContext.ObjectInstance as ArtistViewModel)?.Artist
                ?? (validationContext.ObjectInstance as AlbumViewModel)?.Artist;

            if (selectedArtist.Name != name && existingArtists.Any(a => a.Name == name))
            {
                return new ValidationResult(string.Format("The name entered for {0} already exists in the library - please select the existing artist from the dropdown list", validationContext.DisplayName));
            }

            return ValidationResult.Success;
        }
    }
}
