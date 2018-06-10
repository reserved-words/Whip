using Whip.Common.Model;

namespace Whip.Web.Models
{
    public class ArtistViewModel
    {
        private readonly Artist _unknown;
        public ArtistViewModel(Artist artist, string playUrl)
        {
            Name = artist.Name;
            PlayUrl = playUrl;
        }

        public string Name { get; }
        public string PlayUrl { get; }
    }
}