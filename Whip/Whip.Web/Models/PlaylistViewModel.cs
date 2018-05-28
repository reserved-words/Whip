using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Whip.Web.Models
{
    public class PlaylistViewModel
    {
        public PlaylistViewModel()
        {
            Tracks = new List<TrackViewModel>();
        }

        public int Id { get; set; }
        public string Title { get; set; }
        public List<TrackViewModel> Tracks { get; set; }
    }
}