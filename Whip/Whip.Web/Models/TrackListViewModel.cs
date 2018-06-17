using System.Collections.Generic;

namespace Whip.Web.Models
{
    public class TrackListViewModel
    {
        public TrackListViewModel(List<TrackViewModel> tracks, bool panelHasFooter)
        {
            Tracks = tracks;
            PanelHasFooter = panelHasFooter;
        }

        public List<TrackViewModel> Tracks { get; }
        public bool PanelHasFooter { get; }
    }
}