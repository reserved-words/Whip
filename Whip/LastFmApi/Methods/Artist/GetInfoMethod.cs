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

            artistInfo.Name = artistXml.Element("name").Value.Trim();
            
            artistInfo.Wiki = artistXml.Element("bio").Element("summary").Value.Trim();

            PopulateImages(artistInfo, artistXml.Elements("image"));
            
            PopulateSimilarArtists(artistInfo, artistXml.Element("similar"));

            return artistInfo;
        }

        private void PopulateImages(ArtistInfo artistInfo, IEnumerable<XElement> imageElements)
        {
            foreach (var image in imageElements)
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
        }

        private void PopulateSimilarArtists(ArtistInfo artistInfo, XElement similarArtistsElement)
        {
            artistInfo.SimilarArtists = new List<ArtistInfo>();

            if (similarArtistsElement == null)
                return;

            foreach (var artist in similarArtistsElement.Elements("artist"))
            {
                var similarArtistInfo = new ArtistInfo
                {
                    Name = artist.Element("name").Value.Trim(),
                    Url = artist.Element("url").Value.Trim()
                };

                PopulateImages(similarArtistInfo, artist.Elements("image"));

                artistInfo.SimilarArtists.Add(similarArtistInfo);
            }
        }
    }
}
