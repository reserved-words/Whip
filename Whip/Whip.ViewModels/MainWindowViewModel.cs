using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Messaging;
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

        public MainWindowViewModel(ILibraryService libraryService, IUserSettings userSettings, Library library,
            IMessenger messenger, IPlaylist playlist, MainViewModel mainViewModel, SidebarViewModel sidebarViewModel)
        {
            _libraryService = libraryService;
            _userSettings = userSettings;
            _messenger = messenger;

            _library = library;

            MainViewModel = mainViewModel;
            SidebarViewModel = sidebarViewModel;

            playlist.CurrentTrackChanged += OnCurrentTrackChanged;
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
            OnPopulateLibrary();
        }

        public void OnExit()
        {
            OnSaveLibrary();
        }

        private void OnPopulateLibrary()
        {
            if (!_userSettings.EssentialSettingsSet)
            {
                MainViewModel.SelectTab(TabType.Settings);

                _messenger.Send(new ShowDialogMessage(_messenger, MessageType.Info, UserSettingsMissingTitle, UserSettingsMissingText));
                
                return;
            }

            _messenger.Send(new LibraryUpdateRequest());
        }

        private void OnSaveLibrary()
        {
            _libraryService.SaveLibrary(_library);
        }
    }
}
