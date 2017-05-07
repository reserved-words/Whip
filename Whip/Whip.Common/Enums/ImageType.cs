using System;
using Whip.Common.ExtensionMethods;

namespace Whip.Common.Enums
{
    public enum ImageType
    {
        [Extensions(".jpg",".jpeg")]
        Jpeg,
        [Extensions(".gif")]
        Gif,
        [Extensions(".png")]
        Png,
        [Extensions(".ico")]
        Ico,
        [Extensions(".bmp")]
        Bmp
    }

    public class ExtensionsAttribute : Attribute
    {
        public ExtensionsAttribute(params string[] extensions)
            : base()
        {
            Extensions = extensions;
        }

        public string[] Extensions { get; private set; }
    }

    public static class ImageTypeExtensionMethods
    {
        public static string[] GetValidExtensions(this ImageType imageType)
        {
            return imageType.GetAttribute<ExtensionsAttribute>()?.Extensions ?? new string[0];
        }
    }
}
