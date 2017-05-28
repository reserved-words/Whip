using LastFmApi.Internal;
using System.Collections.Generic;
using System.Xml.Linq;

namespace LastFmApi.Methods.Artist
{
    internal class GetInfoMethod : ApiMethodBase<ArtistInfo>
    {
        public GetInfoMethod(ApiClient client, string artistName)
            : base(client, "artist.getinfo")
        {
            SetParameters(new Dictionary<ParameterKey, string>
            {
                { ParameterKey.Artist, artistName }
            });
        }

        public override ArtistInfo ParseResult(XElement xml)
        {
            var artistXml = xml.Element("artist");

            var artistInfo = new ArtistInfo();

            var artistName = artistXml.Element("name").Value.Trim();
            
            artistInfo.Wiki = artistXml.Element("bio").Element("summary").Value.Trim();

            foreach (var image in artistXml.Elements("image"))
            {
                switch (image.Attribute("size").Value)
                {
                    case "small":
                        artistInfo.SmallImageUrl = image.Value.Trim();
                        break;
                    case "medium":
                        artistInfo.MediumImageUrl = image.Value.Trim();
                        break;
                    case "large":
                        artistInfo.LargeImageUrl = image.Value.Trim();
                        break;
                    case "mega":
                        artistInfo.ExtraLargeImageUrl = image.Value.Trim();
                        break;
                }
            }

            return artistInfo;
        }
    }
}
