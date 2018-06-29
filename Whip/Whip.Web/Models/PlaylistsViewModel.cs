using System.Collections.Generic;

namespace Whip.Web.Models
{
    public class PlaylistsViewModel
    {
        public PlaylistsViewModel()
        {
            StandardPlaylists = new List<PlaylistViewModel>();
            CriteriaPlaylists = new List<PlaylistViewModel>();
            OrderedPlaylists = new List<PlaylistViewModel>();
        }

        public List<PlaylistViewModel> StandardPlaylists { get; set; }
        public List<PlaylistViewModel> CriteriaPlaylists { get; set; }
        public List<PlaylistViewModel> OrderedPlaylists { get; set; }
    }
}