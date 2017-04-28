using LastFmApi.Internal;
using System.Collections.Generic;
using System.Xml.Linq;

namespace LastFmApi.Methods.Auth
{
    internal class GetSessionMethod : ApiMethodBase<Session>
    {
        private readonly string _username;

        public GetSessionMethod(AuthorizedApiClient client, string token)
            : base(client, "auth.getSession")
        {
            _username = client.Username;

            SetParameters(new Dictionary<ParameterKey, string>
            {
                { ParameterKey.Token, token }
            });
        }

        public override Session ParseResult(XElement xml)
        {
            var sessionElement = xml.Element("session");

            if (sessionElement == null)
                return null;

            string username = sessionElement.Element("name").Value;
            string key = sessionElement.Element("key").Value;

            return new Session(key, username);
        }
    }
}
