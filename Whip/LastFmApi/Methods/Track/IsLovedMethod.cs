using LastFmApi.Internal;
using System.Xml.Linq;

namespace LastFmApi.Methods.Track
{
    internal class IsLovedMethod : ApiMethodBase<bool>
    {
        public IsLovedMethod(ApiClient client, LastFmApi.Track track, string username)
            : base(client, "track.getInfo")
        {
            Parameters.Add(ParameterKey.Track, track.Title);
            Parameters.Add(ParameterKey.Artist, track.Artist);
            Parameters.Add(ParameterKey.Album, track.Album);
            Parameters.Add(ParameterKey.AlbumArtist, track.AlbumArtist);
            Parameters.Add(ParameterKey.Username, username);
        }

        public override bool ParseResult(string result)
        {
            var doc = XDocument.Parse(result);
            return doc.Element("lfm").Element("track").Element("userloved").Value == "1";
        }
    }
}
