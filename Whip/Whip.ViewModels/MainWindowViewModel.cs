using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Messaging;
using Whip.Common.Interfaces;
using Whip.Common.Model;
using Whip.Common.Singletons;
using Whip.Services.Interfaces;
using Whip.Services.Interfaces.Singletons;
using Whip.ViewModels.Messages;
using Whip.ViewModels.Utilities;

namespace Whip.ViewModels
{
    public class MainWindowViewModel : ViewModelBase
    {
        private const string UserSettingsMissingTitle = "User Settings";
        private const string UserSettingsMissingText = "Please populate the essential settings for the music player to work correctly";

        private readonly ILibraryService _libraryService;
        private readonly IUserSettings _userSettings;
        private readonly IMessenger _messenger;

        private readonly Library _library;
        private readonly IPlayer _player;

        public MainWindowViewModel(ILibraryService libraryService, IUserSettings userSettings, Library library,
            IMessenger messenger, IPlaylist playlist, MainViewModel mainViewModel, SidebarViewModel sidebarViewModel,
            IPlayer player)
        {
            _libraryService = libraryService;
            _userSettings = userSettings;
            _messenger = messenger;

            _library = library;
            _player = player;

            MainViewModel = mainViewModel;
            SidebarViewModel = sidebarViewModel;

            playlist.CurrentTrackChanged += OnCurrentTrackChanged;
            MainViewModel.FavouritePlaylistsUpdated += OnFavouritePlaylistsUpdated;
        }

        private void OnFavouritePlaylistsUpdated()
        {
            SidebarViewModel.LoadPlaylists();
        }

        private void OnCurrentTrackChanged(Track track)
        {
            SidebarViewModel.OnCurrentTrackChanged(track);
            MainViewModel.OnCurrentTrackChanged(track);
        }

        public MainViewModel MainViewModel { get; }
        public SidebarViewModel SidebarViewModel { get; }

        public void OnLoad()
        {
            PopulateLibrary();

            SidebarViewModel.LoadPlaylists();
        }

        public void OnExit()
        {
            _player.Stop();
            SaveLibrary();
        }

        private void PopulateLibrary()
        {
            if (!_userSettings.EssentialSettingsSet)
            {
                MainViewModel.SelectTab(TabType.Settings);

                _messenger.Send(new ShowDialogMessage(_messenger, MessageType.Info, UserSettingsMissingTitle, UserSettingsMissingText));
                
                return;
            }

            _messenger.Send(new LibraryUpdateRequest());
        }

        private void SaveLibrary()
        {
            _libraryService.SaveLibrary(_library);
        }
    }
}
