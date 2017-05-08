using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Whip.Common.Model;

namespace Whip.ViewModels.TabViewModels.Playlists
{
    public class CriteriaPlaylistsViewModel
    {
        public CriteriaPlaylistsViewModel()
        {
            Playlists = new ObservableCollection<CriteriaPlaylist>();
        }

        public void Update(List<CriteriaPlaylist> playlists)
        {
            Playlists.Clear();
            playlists.ForEach(Playlists.Add);
        }

        public ObservableCollection<CriteriaPlaylist> Playlists { get; set; }
    }
    
    
}
