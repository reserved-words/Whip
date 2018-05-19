using System;
using System.Linq;
using System.Windows.Markup;
using Whip.Common.ExtensionMethods;

namespace Whip
{
    public class EnumerationExtension : MarkupExtension
    {
        private Type _enumType;
        
        public EnumerationExtension(Type enumType)
        {
            if (enumType == null)
                throw new ArgumentNullException("enumType");

            EnumType = enumType;
        }

        public Type EnumType
        {
            get { return _enumType; }
            private set
            {
                if (_enumType == value)
                    return;

                var enumType = Nullable.GetUnderlyingType(value) ?? value;

                if (!enumType.IsEnum)
                    throw new ArgumentException("Type must be an Enum.");

                _enumType = value;
            }
        }

        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            var members = from Enum enumValue in Enum.GetValues(EnumType)
                          select new EnumerationMember
                          {
                              Value = enumValue,
                              ValueString = enumValue.ToString(),
                              Description = enumValue.GetDisplayName()
                          };

            return members.ToArray();
        }

        public class EnumerationMember
        {
            public string Description { get; set; }
            public object Value { get; set; }
            public string ValueString { get; set; }
        }
    }
}
