using System.Collections.Generic;
using System.Linq;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using Whip.Common;
using Whip.Common.ExtensionMethods;
using Whip.Common.Model;
using Whip.Common.Singletons;
using Whip.Services.Interfaces;

namespace Whip.ViewModels.SidebarViewModels
{
    public class PlayShortcutsViewModel : ViewModelBase
    {
        public enum PlayerStatus { Playing, Paused, Stopped }

        private readonly Library _library;
        private readonly IPlayRequestHandler _playRequestHandler;

        private List<string> _groupings;

        public PlayShortcutsViewModel(Library library, IPlayRequestHandler playRequestHandler)
        {
            _library = library;
            _playRequestHandler = playRequestHandler;

            _library.Updated += OnLibraryUpdated;

            PlayGroupingCommand = new RelayCommand<string>(OnShuffleGrouping);
            ShuffleLibraryCommand = new RelayCommand(OnShuffleLibrary);
        }

        public List<string> Groupings
        {
            get { return _groupings; }
            set { Set(ref _groupings, value); }
        }

        public RelayCommand<string> PlayGroupingCommand { get; }
        public RelayCommand ShuffleLibraryCommand { get; }
        
        private void OnLibraryUpdated(Track track)
        {
            Groupings = _library.GetGroupings().Where(g => !string.IsNullOrEmpty(g)).ToList();
        }

        private void OnShuffleGrouping(string grouping)
        {
            _playRequestHandler.PlayGrouping(grouping, SortType.Random);
        }

        private void OnShuffleLibrary()
        {
            _playRequestHandler.PlayAll(SortType.Random);
        }
    }
}
