using GalaSoft.MvvmLight;
using Whip.Common.Model;

namespace Whip.ViewModels.TabViewModels.Playlists
{
    public class QuickPlaylistViewModel : ViewModelBase
    {
        public QuickPlaylistViewModel(QuickPlaylist playlist)
        {
            Playlist = playlist;
        }
        
        public QuickPlaylist Playlist { get; }

        public string Title => Playlist.Title;

        public bool Favourite
        {
            get { return Playlist.Favourite; }
            set
            {
                Playlist.Favourite = value;
                RaisePropertyChanged(nameof(Favourite));
            }
        }
    }
}
