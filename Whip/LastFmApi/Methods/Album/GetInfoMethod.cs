using LastFmApi.Internal;
using System.Linq;
using System.Xml.Linq;

namespace LastFmApi.Methods.Album
{
    internal class GetInfoMethod : ApiMethodBase<AlbumInfo>
    {
        public GetInfoMethod(ApiClient client, string artistName, string albumTitle)
            : base(client, "album.getinfo")
        {
            Parameters.Add(ParameterKey.Artist, artistName);
            Parameters.Add(ParameterKey.Album, albumTitle);
        }

        public override AlbumInfo ParseResult(string result)
        {
            var xml = XDocument.Parse(result);

            var albumXml = xml.Element("lfm").Element("album");

            var artworkUrl = albumXml.Elements("image")
                .Where(el => el.Attribute("size").Value == "mega")
                .SingleOrDefault()
                .Value.Trim();

            return new AlbumInfo(artworkUrl);
        }
    }
}
