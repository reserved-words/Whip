using GalaSoft.MvvmLight;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Whip.Common.Model;

namespace Whip.ViewModels.TabViewModels.Playlists
{
    public class StandardFilterViewModel : ViewModelBase
    {
        private QuickPlaylist _selectedPlaylist;

        public StandardFilterViewModel(string title, List<QuickPlaylist> list)
        {
            Title = title;
            List = list;
            SelectedPlaylist = null;
        }

        public string Title { get; }
        public List<QuickPlaylist> List { get; }
        public bool IsFavouriteSelected => SelectedPlaylist?.Favourite == true;

        public QuickPlaylist SelectedPlaylist
        {
            get { return _selectedPlaylist; }
            set
            {
                Set(ref _selectedPlaylist, value);
                RaisePropertyChanged(nameof(IsFavouriteSelected));
            }
        }
    }
}
