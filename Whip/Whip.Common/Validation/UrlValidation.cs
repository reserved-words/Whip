using System;

namespace Whip.Common.Validation
{
    public static class UrlValidation
    {
        public const byte PartialUrlStringMaxLength = 255;
        public const string PartialUrlStringRegexPattern = @"^[A-Za-z0-9-_~\?#\[\]\@\!\$&\'\(\)\*\+,\=%\.]*$";

        public static bool IsValidUrl(string value)
        {
            if (string.IsNullOrEmpty(value))
                return true;

            Uri uriResult;
            return Uri.TryCreate(value, UriKind.Absolute, out uriResult)
                && (uriResult.Scheme == Uri.UriSchemeHttp || uriResult.Scheme == Uri.UriSchemeHttps);
        }
    }
}
