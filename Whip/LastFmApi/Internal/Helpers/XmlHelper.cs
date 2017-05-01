using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace LastFmApi.Internal
{
    internal static class XmlHelper
    {
        internal static XElement Validate(string response)
        {
            // TEST ERROR
            // throw new LastFmApiException(ErrorCode.ServiceTemporarilyUnavailable, "Last.FM temporarily unavailable");
            // throw new LastFmApiException(ErrorCode.InvalidApiKey, "Invalid API Key");

            var xml = XDocument.Parse(response);

            var root = xml.Element("lfm");
            if (root.Attribute("status").Value == "failed")
            {
                var error = root.Element("error");
                throw new LastFmApiException(error.Attribute("code").Value, error.Value);
            }

            return root;
        }
    }
}
