using System.Text.RegularExpressions;

namespace Whip.Common.Validation
{
    public static class UrlValidation
    {
        public const string UrlRegexPattern = @"(http://|https://)?(www\.)?\w+\.(com|net|edu|org|co\.uk)"; // Need to make this less restrictive

        public const byte PartialUrlStringMaxLength = 255;
        public const string PartialUrlStringRegexPattern = @"^[A-Za-z0-9-_~\?#\[\]\@\!\$&\'\(\)\*\+,\=%]*$";

        public static bool IsValidUrl(string value)
        {
            if (string.IsNullOrEmpty(value))
                return true;

            if (Regex.IsMatch(value, UrlRegexPattern))
                return true;

            return false;
        }

        public static bool IsValidArtworkUrl(string value)
        {
            if (value == null)
                return true;

            value = value.ToLower();

            return IsValidUrl(value) && (value.EndsWith(".jpg") || value.EndsWith(".jpeg") || value.EndsWith(".bmp") || value.EndsWith(".png"));
        }
    }
}
