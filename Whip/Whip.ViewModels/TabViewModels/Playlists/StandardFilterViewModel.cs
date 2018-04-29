using GalaSoft.MvvmLight;
using System.Collections.Generic;

namespace Whip.ViewModels.TabViewModels.Playlists
{
    public class StandardFilterViewModel : ViewModelBase
    {
        private QuickPlaylistViewModel _selectedPlaylist;

        public StandardFilterViewModel(string title, List<QuickPlaylistViewModel> list)
        {
            Title = title;
            List = list;
            SelectedPlaylist = null;
        }

        public string Title { get; }
        public List<QuickPlaylistViewModel> List { get; }
        public bool IsFavouriteSelected => SelectedPlaylist?.Favourite == true;

        public QuickPlaylistViewModel SelectedPlaylist
        {
            get { return _selectedPlaylist; }
            set
            {
                Set(ref _selectedPlaylist, value);
                RaisePropertyChanged(nameof(IsFavouriteSelected));
            }
        }

        public void SetSelectedPlaylistFavourite(bool favourite)
        {
            SelectedPlaylist.Favourite = favourite;
            RaisePropertyChanged(nameof(IsFavouriteSelected));
        }
    }
}
