using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using LastFmApi.Internal;
using LastFmApi.Model;

namespace LastFmApi.Methods.User
{
    internal class GetRecentTracksMethod : ApiMethodBase<ICollection<TrackPlayInfo>>
    {
        public GetRecentTracksMethod(ApiClient client, string username, int limit)
            : base(client, "user.getrecenttracks")
        {
            SetParameters(new Dictionary<ParameterKey, string>
            {
                { ParameterKey.User, username },
                { ParameterKey.Limit, limit.ToString() }
            });
        }

        public override ICollection<TrackPlayInfo> ParseResult(XElement xml)
        {
            var recentTracksXml = xml.Element("recenttracks");
            var list = new List<TrackPlayInfo>();

            foreach (var trackPlayXml in recentTracksXml.Elements("track"))
            {
                try
                {
                    var trackPlay = new TrackPlayInfo
                    {
                        TrackTitle = trackPlayXml.Element("name")?.Value,
                        ArtistName = trackPlayXml.Element("artist")?.Value,
                        AlbumTitle = trackPlayXml.Element("album")?.Value,
                        Url = trackPlayXml.Element("url")?.Value,
                        NowPlaying = trackPlayXml.Attribute("nowplaying")?.Value == "true",
                        ImageUrl = trackPlayXml.Elements("image")
                            .FirstOrDefault(im => im.Attribute("size").Value == "large")?.Value
                    };

                    trackPlay.TimePlayed = trackPlay.NowPlaying
                        ? (DateTime?)null
                        : DateTime.Parse(trackPlayXml.Element("date").Value);

                    list.Add(trackPlay);
                }
                catch (Exception ex)
                {

                    throw;
                }
            }

            return list;
        }
    }
}
