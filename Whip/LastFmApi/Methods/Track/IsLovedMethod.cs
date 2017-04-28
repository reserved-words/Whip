using LastFmApi.Internal;
using System.Collections.Generic;
using System.Xml.Linq;

namespace LastFmApi.Methods.Track
{
    internal class IsLovedMethod : ApiMethodBase<bool>
    {
        public IsLovedMethod(ApiClient client, LastFmApi.Track track, string username)
            : base(client, "track.getInfo")
        {
            SetParameters(new Dictionary<ParameterKey, string>
            {
                { ParameterKey.Track, track.Title },
                { ParameterKey.Artist, track.Artist },
                { ParameterKey.Album, track.Album },
                { ParameterKey.AlbumArtist, track.AlbumArtist },
                { ParameterKey.Username, username }
            });
        }

        public override bool ParseResult(XElement xml)
        {
            return xml.Element("track").Element("userloved").Value == "1";
        }
    }
}
