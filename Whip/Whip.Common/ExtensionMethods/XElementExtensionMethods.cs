using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Whip.Common.ExtensionMethods
{
    public static class XElementExtensionMethods
    {
        public static string GetValue(this XElement xml, string key)
        {
            return xml.Element(key)?.Value ?? string.Empty;
        }

        public static string GetAttribute(this XElement xml, string key)
        {
            return xml.Attribute(key)?.Value ?? string.Empty;
        }

        public static int GetIntAttribute(this XElement xml, string key)
        {
            var attr = xml.Attribute(key);

            if (attr == null)
                return 0;

            int result;
            if (!int.TryParse(attr.Value, out result))
                return 0;

            return result;
        }

        public static DateTime GetDateTimeAttribute(this XElement xml, string key)
        {
            var attr = xml.Attribute(key);

            if (attr == null)
                return DateTime.MinValue;

            DateTime result;
            if (!DateTime.TryParse(attr.Value, out result))
                return DateTime.MinValue;

            return result;
        }

        public static void AddAttribute(this XElement xml, string key, string value)
        {
            xml.Add(new XAttribute(key, value ?? string.Empty));
        }

        public static void AddAttribute(this XElement xml, string key, int value)
        {
            xml.Add(new XAttribute(key, value));
        }
    }
}
