using LastFmApi.Internal;
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

            Parameters.Add(ParameterKey.Token, token);

            AddApiSignature();
        }

        public override Session ParseResult(string result)
        {
            var doc = XDocument.Parse(result);
            var sessionElement = doc.Element("lfm").Element("session");

            if (sessionElement == null)
                return null;

            string username = sessionElement.Element("name").Value;
            string key = sessionElement.Element("key").Value;

            return new Session(key, username);
        }
    }
}
