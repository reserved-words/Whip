using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using LastFmApi.Internal;
using LastFmApi.Model;

namespace LastFmApi.Methods.User
{
    internal class GetTopAlbumsMethod : ApiMethodBase<ICollection<UserAlbumInfo>>
    {
        public GetTopAlbumsMethod(ApiClient client, string username, TimePeriod timePeriod, int limit)
            : base(client, "user.gettopalbums")
        {
            SetParameters(new Dictionary<ParameterKey, string>
            {
                { ParameterKey.User, username },
                { ParameterKey.Period, timePeriod.GetStringValue() },
                { ParameterKey.Limit, limit.ToString() }
            });
        }

        public override ICollection<UserAlbumInfo> ParseResult(XElement xml)
        {
            var resultsXml = xml.Element("topalbums");
            var list = new List<UserAlbumInfo>();

            foreach (var result in resultsXml.Elements("album"))
            {
                list.Add(new UserAlbumInfo
                {
                    Title = result.Element("name")?.Value,
                    ArtistName = result.Element("artist").Element("name").Value,
                    Url = result.Element("url").Value,
                    PlayCount = int.Parse(result.Element("playcount").Value),
                    ImageUrl = result.Elements("image").FirstOrDefault(im => im.Attribute("size").Value == "large")?.Value
                });
            }

            return list;
        }
    }
}
