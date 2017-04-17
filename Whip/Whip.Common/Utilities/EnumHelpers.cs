using System;
using System.Linq;

namespace Whip.Common.Utilities
{
    public static class EnumHelpers
    {
        public static TEnum Parse<TEnum>(string str) where TEnum : struct
        {
            TEnum value;
            if (Enum.TryParse(str, out value))
            {
                return value;
            }

            return GetDefaultValue<TEnum>();
        }

        public static TEnum GetDefaultValue<TEnum>() where TEnum : struct
        {
            var attributes = typeof(TEnum)
                .GetCustomAttributes(typeof(DefaultValueAttribute), false)
                .OfType<DefaultValueAttribute>();

            return (TEnum)(attributes.FirstOrDefault()?.Value ?? default(TEnum));
        }
    }
}
