using Whip.Common.ExtensionMethods;
using Whip.Common.Model;

namespace Whip.Web.ExtensionMethods
{
    public static class ArtistExtensionMethods
    {
        public static string Category(this Artist artist)
        {
            return string.IsNullOrEmpty(artist.Sort) || artist.Sort.GetFirstWord().IsInteger()
                ? "#"
                : artist.Sort.Substring(0, 1).ToUpperInvariant();
        }
    }
}