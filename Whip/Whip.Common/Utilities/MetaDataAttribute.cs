using System;

namespace Whip.Common.Utilities
{
    public class MetaDataAttribute : Attribute
    {
        public MetaDataAttribute(string displayName)
        {
            DisplayName = displayName;
        }

        public string DisplayName { get; private set; }
    }
}
