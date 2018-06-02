using System.Xml.Linq;

namespace LastFmApi.Methods.Auth
{
    internal class GetTokenMethod : ApiMethodBase<string>
    {
        public GetTokenMethod(UserApiClient client)
            : base(client, "auth.getToken")
        {
            SetParameters();
        }

        public override string ParseResult(XElement xml)
        {
            return xml.Element("token").Value;
        }
    }
}
