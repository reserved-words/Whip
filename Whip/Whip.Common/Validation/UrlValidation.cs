using System;

namespace Whip.Common.Validation
{
    public static class UrlValidation
    {
        public const byte PartialUrlStringMaxLength = 255;
        public const string PartialUrlStringRegexPattern = @"^[A-Za-z0-9-_~\?#\[\]\@\!\$&\'\(\)\*\+,\=%]*$";

        public static bool IsValidUrl(string value)
        {
            if (string.IsNullOrEmpty(value))
                return true;

            Uri uriResult;
            return Uri.TryCreate(value, UriKind.Absolute, out uriResult)
                && (uriResult.Scheme == Uri.UriSchemeHttp || uriResult.Scheme == Uri.UriSchemeHttps);
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
