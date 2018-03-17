using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using LastFmApi.Internal;
using LastFmApi.Model;

namespace LastFmApi.Methods.User
{
    internal class GetTopArtistsMethod : ApiMethodBase<ICollection<UserArtistInfo>>
    {
        public GetTopArtistsMethod(ApiClient client, string username, TimePeriod timePeriod, int limit)
            : base(client, "user.gettopartists")
        {
            SetParameters(new Dictionary<ParameterKey, string>
            {
                { ParameterKey.User, username },
                { ParameterKey.Period, timePeriod.GetStringValue() },
                { ParameterKey.Limit, limit.ToString() }
            });
        }

        public override ICollection<UserArtistInfo> ParseResult(XElement xml)
        {
            var topArtistsXml = xml.Element("topartists");
            var list = new List<UserArtistInfo>();

            foreach (var artistXml in topArtistsXml.Elements("artist"))
            {
                list.Add(new UserArtistInfo
                {
                    Name = artistXml.Element("name")?.Value,
                    Url = artistXml.Element("url").Value,
                    PlayCount = int.Parse(artistXml.Element("playcount").Value),
                    ImageUrl = artistXml.Elements("image").FirstOrDefault(im => im.Attribute("size").Value == "large")?.Value
                });
            }

            return list;
        }
    }
}
