using System;
using System.Linq;

namespace Whip.Common.Utilities
{
    public static class EnumExtensionMethods
    {
        public static T GetAttribute<T>(this Enum en) where T : Attribute
        {
            var memInfo = en.GetType().GetMember(en.ToString());

            if (memInfo == null || !memInfo.Any())
                return null;

            var attrs = memInfo[0].GetCustomAttributes(typeof(T), false).OfType<T>();

            return attrs.FirstOrDefault();
        }

        public static string GetDisplayName(this Enum en)
        {
            return en.GetAttribute<MetaDataAttribute>()?.DisplayName ?? en.ToString();
        }
    }
}
