using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Messaging;
using Whip.Common.Model;
using Whip.Common.Singletons;
using Whip.Services.Interfaces;
using Whip.ViewModels.Messages;
using Whip.ViewModels.Utilities;
using Whip.ViewModels.Windows;

namespace Whip.ViewModels
{
    public class MainWindowViewModel : ViewModelBase
    {
        private const string UserSettingsMissingTitle = "User Settings";
        private const string UserSettingsMissingText = "Please populate the essential settings for the music player to work correctly";

        private readonly ILibraryService _libraryService;
        private readonly IUserSettingsService _userSettingsService;
        private readonly IMessenger _messenger;

        private readonly Library _library;
        private readonly Playlist _playlist;

        public MainWindowViewModel(ILibraryService libraryService, IUserSettingsService userSettingsService, Library library,
            IMessenger messenger, Playlist playlist, ITrackFilterService trackFilterService,
            MainViewModel mainViewModel, SidebarViewModel sidebarViewModel)
        {
            _libraryService = libraryService;
            _userSettingsService = userSettingsService;
            _messenger = messenger;

            _library = library;
            _playlist = playlist;

            MainViewModel = mainViewModel;
            SidebarViewModel = sidebarViewModel;

            _playlist.CurrentTrackChanged += OnCurrentTrackChanged;
        }

        private void OnCurrentTrackChanged(Track track)
        {
            SidebarViewModel.OnCurrentTrackChanged(track);
            MainViewModel.OnCurrentTrackChanged(track);
        }

        public MainViewModel MainViewModel { get; private set; }
        public SidebarViewModel SidebarViewModel { get; private set; }

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
            if (!_userSettingsService.EssentialSettingsSet)
            {
                MainViewModel.SelectTab(TabType.Settings);

                var messageViewModel = new MessageViewModel(_messenger, UserSettingsMissingTitle, UserSettingsMissingText);
                _messenger.Send(new ShowDialogMessage(messageViewModel));
                
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
