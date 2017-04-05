using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Messaging;
using System;
using Whip.Common.Utilities;
using Whip.Services.Interfaces;
using Whip.ViewModels.Messages;
using Whip.ViewModels.Singletons.Interfaces;
using Whip.ViewModels.Windows;

namespace Whip.ViewModels
{
    public class MainWindowViewModel
    {
        private readonly ILibraryService _libraryService;
        private readonly IUserSettingsService _userSettingsService;
        private readonly ILibrary _library;
        private readonly IMessenger _messenger;

        public MainWindowViewModel(ILibraryService libraryService, IUserSettingsService userSettingsService, ILibrary library,
            IMessenger messenger)
        {
            _libraryService = libraryService;
            _userSettingsService = userSettingsService;
            _library = library;
            _messenger = messenger;

            PopulateLibraryCommand = new RelayCommand(OnPopulateLibrary);
            SaveLibraryCommand = new RelayCommand(OnSaveLibrary);
        }
        
        public MainViewModel MainViewModel => new MainViewModel();
        public SidebarViewModel SidebarViewModel => new SidebarViewModel();

        public RelayCommand PopulateLibraryCommand { get; private set; }
        public RelayCommand SaveLibraryCommand { get; private set; }

        private void OnPopulateLibrary()
        {
            var guid = new Guid();
            var progressBarViewModel = new ProgressBarViewModel("Populating Library");
            var startProgressBarMessage = new ShowDialogMessage(guid, progressBarViewModel);

            OnPopulateLibraryAsync(guid, progressBarViewModel);

            _messenger.Send(startProgressBarMessage);
        }

        private async void OnPopulateLibraryAsync(Guid guid, ProgressBarViewModel progressBarViewModel)
        {
            var progressHandler = new Progress<ProgressArgs>(progressBarViewModel.Update);
            var stopProgressBarMessage = new HideDialogMessage(guid);

            _library.Artists = await _libraryService.GetLibraryAsync(_userSettingsService.MusicDirectory, ApplicationSettings.FileExtensions, progressHandler);

            _messenger.Send(stopProgressBarMessage);
        }

        private void OnSaveLibrary()
        {
            if (_library.Artists == null)
            {
                OnPopulateLibrary();
            }

            _libraryService.SaveLibrary(_library.Artists);
        }
    }
}
