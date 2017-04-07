using System;

namespace Whip.Common.Utilities
{
    public class DefaultValueAttribute : Attribute
    {
        public DefaultValueAttribute(object value)
        {
            Value = value;
        }

        public object Value { get; private set; }
    }
}
