using System;

namespace Whip.Common.Utilities
{
    public class MetaDataAttribute : Attribute
    {
        public MetaDataAttribute(string displayName)
        {
            DisplayName = displayName;
        }

        public MetaDataAttribute(string displayName, IconType iconType, bool visible = true)
            :this(displayName)
        {
            IconType = iconType;
            Visible = visible;
        }

        public string DisplayName { get; private set; }
        public IconType? IconType { get; private set; }
        public bool Visible { get; private set; }
    }
}
