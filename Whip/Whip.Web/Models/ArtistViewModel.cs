using Whip.Common.Model;

namespace Whip.Web.Models
{
    public class ArtistViewModel : PlayItemViewModel
    {
        public ArtistViewModel(Artist artist, string playUrl, string infoUrl)
            :base(artist.Name, playUrl, infoUrl)
        {
        }
    }
}