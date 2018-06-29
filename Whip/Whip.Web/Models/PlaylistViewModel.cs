using System.Collections.Generic;

namespace Whip.Web.Models
{
    public class PlaylistViewModel : PlayItemViewModel
    {
        public PlaylistViewModel(string title, string playUrl, string infoUrl)
            : base(title, playUrl, infoUrl)
        {
        }
    }
}