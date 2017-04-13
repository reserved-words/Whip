using System;

namespace Whip.Common.Utilities
{
    public class MetaDataAttribute : Attribute
    {
        public MetaDataAttribute(string displayName)
        {
            DisplayName = displayName;
        }

        public MetaDataAttribute(string displayName, IconType iconType)
            :this(displayName)
        {
            IconType = iconType;
        }

        public string DisplayName { get; private set; }
        public IconType? IconType { get; private set; }
    }
}
