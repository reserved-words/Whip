using Whip.Common.Model;

namespace Whip.Web.Models
{
    public class AlbumViewModel : PlayItemViewModel
    {
        public AlbumViewModel(Album album, string playUrl, string infoUrl)
            :base($"{album.Title} ({album.Year})", playUrl, infoUrl)
        {
        }
    }
}