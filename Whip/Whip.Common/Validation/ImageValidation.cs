using System.Linq;
using Whip.Common.Enums;

namespace Whip.Common.Validation
{
    public static class ImageValidation
    {
        public static bool HasValidImageExtension(string value, params ImageType[] types)
        {
            if (value == null)
                return true;

            value = value.ToLower();

            var validExtensionTypes = types.SelectMany(t => t.GetValidExtensions());

            return validExtensionTypes.Any(ext => value.EndsWith(ext));
        }
    }
}
