
using System.Text.RegularExpressions;

namespace Whip.Common
{
    public static class Validation
    {
        public const string MaxLengthErrorMessage = "{0} must not be more than {1} characters";
        public const string RangeErrorMessage = "{0} must be between {1} and {2}";

        public const string UrlRegexPattern = @"(http://|https://)?(www\.)?\w+\.(com|net|edu|org)";

        public const byte PartialUrlStringMaxLength = 255;
        public const string PartialUrlStringRegexPattern = @"^[A-Za-z0-9-_~\?#\[\]\@\!\$&\'\(\)\*\+,\=%]*$";

        public const byte FacebookUsernameMaxLength = 15;
        public const string FacebookUsernameRegexPattern = @"^[0-9a-zA-Z\.]*$";

        public const byte TwitterUsernameMaxLength = 15;
        public const string TwitterUsernameRegexPattern = "^[0-9a-zA-Z_]*$";

        public const byte MaxLengthTrackTitle = 255;
        public const byte MaxLengthAlbumTitle = 255;
        public const byte MaxLengthArtistName = 255;
        public const byte MaxLengthGrouping = 30;
        public const byte MaxLengthGenre = 30;
        public const byte MaxLengthCountry = 30;
        public const byte MaxLengthState = 30;
        public const byte MaxLengthCity = 30;
        public const int MaxLengthLyrics = 4000;

        public const int MinTrackCount = 1;
        public const int MaxTrackCount = 999;

        public const int MinDiscCount = 1;
        public const int MaxDiscCount = 99;

        public static bool IsValidUrl(string value)
        {
            if (string.IsNullOrEmpty(value))
                return true;

            if (Regex.IsMatch(value, Validation.UrlRegexPattern))
                return true;

            return false;
        }

        public static bool IsValidArtworkUrl(string value)
        {
            if (value == null)
                return true;

            value = value.ToLower();

            return IsValidUrl(value) && (value.EndsWith(".jpg")|| value.EndsWith(".jpeg")|| value.EndsWith(".bmp")|| value.EndsWith(".png"));
        }
    }
}
