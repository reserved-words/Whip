using System.Collections.Generic;

namespace Whip.Web.Models
{
    public class PlayViewModel
    {
        public PlayViewModel()
        {
            Tracks = new List<TrackViewModel>();
        }

        public string Title { get; set; }
        public List<TrackViewModel> Tracks { get; set; }
        public string PlayUrl { get; set; }
    }
}