using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

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
        public TrackViewModel FirstTrack { get; set; }
    }
}