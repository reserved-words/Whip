using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Whip.Common.Model;

namespace Whip.ViewModels.TabViewModels.Playlists
{
    public class OrderedPlaylistsViewModel
    {
        public OrderedPlaylistsViewModel()
        {
            Playlists = new ObservableCollection<OrderedPlaylist>();
        }

        public void Update(List<OrderedPlaylist> playlists)
        {
            Playlists.Clear();
            playlists.ForEach(Playlists.Add);
        }

        public ObservableCollection<OrderedPlaylist> Playlists { get; set; }
    }
}
