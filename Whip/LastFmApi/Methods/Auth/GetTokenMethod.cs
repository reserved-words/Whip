using System.Xml.Linq;

namespace LastFmApi.Methods.Auth
{
    internal class GetTokenMethod : ApiMethodBase<string>
    {
        public GetTokenMethod(AuthorizedApiClient client)
            : base(client, "auth.getToken")
        {
            AddApiSignature();
        }

        public override string ParseResult(string result)
        {
            var doc = XDocument.Parse(result);
            string token = doc.Element("lfm").Element("token").Value;
            return token;
        }
    }
}
