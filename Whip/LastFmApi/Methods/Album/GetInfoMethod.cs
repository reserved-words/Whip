using LastFmApi.Internal;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;

namespace LastFmApi.Methods.Album
{
    internal class GetInfoMethod : ApiMethodBase<AlbumInfo>
    {
        public GetInfoMethod(ApiClient client, string artistName, string albumTitle)
            : base(client, "album.getinfo")
        {
            SetParameters(new Dictionary<ParameterKey, string>
            {
                { ParameterKey.Artist, artistName },
                { ParameterKey.Album, albumTitle }
            });
        }

        public override AlbumInfo ParseResult(XElement xml)
        {
            var albumXml = xml.Element("album");

            var artworkUrl = albumXml.Elements("image")
                .Where(el => el.Attribute("size").Value == "mega")
                .SingleOrDefault()
                .Value.Trim();

            return new AlbumInfo(artworkUrl);
        }
    }
}
