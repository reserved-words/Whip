using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Messaging;
using System;
using Whip.Common.Model;
using Whip.Common.Utilities;
using Whip.Services.Interfaces;
using Whip.ViewModels.Messages;
using Whip.ViewModels.TabViewModels;
using Whip.ViewModels.Windows;

namespace Whip.ViewModels
{
    public class MainWindowViewModel : ViewModelBase
    {
        private readonly ILibraryService _libraryService;
        private readonly IUserSettingsService _userSettingsService;
        private readonly IMessenger _messenger;

        private readonly LibraryViewModel _libraryViewModel;

        private Library _library;

        public MainWindowViewModel(ILibraryService libraryService, IUserSettingsService userSettingsService, Library library,
            IMessenger messenger, LibraryViewModel libraryViewModel)
        {
            _libraryService = libraryService;
            _userSettingsService = userSettingsService;
            _library = library;
            _messenger = messenger;

            _libraryViewModel = libraryViewModel;

            PopulateLibraryCommand = new RelayCommand(OnPopulateLibrary);
            SaveLibraryCommand = new RelayCommand(OnSaveLibrary);
        }
        
        public MainViewModel MainViewModel => new MainViewModel(_libraryViewModel);
        public SidebarViewModel SidebarViewModel => new SidebarViewModel();

        public RelayCommand PopulateLibraryCommand { get; private set; }
        public RelayCommand SaveLibraryCommand { get; private set; }

        private void OnPopulateLibrary()
        {
            var guid = Guid.NewGuid();
            var progressBarViewModel = new ProgressBarViewModel("Populating Library");
            var startProgressBarMessage = new ShowDialogMessage(guid, progressBarViewModel);

            OnPopulateLibraryAsync(guid, progressBarViewModel);

            _messenger.Send(startProgressBarMessage);
        }

        private async void OnPopulateLibraryAsync(Guid guid, ProgressBarViewModel progressBarViewModel)
        {
            var progressHandler = new Progress<ProgressArgs>(progressBarViewModel.Update);
            var stopProgressBarMessage = new HideDialogMessage(guid);

            _library = await _libraryService.GetLibraryAsync(_userSettingsService.MusicDirectory, ApplicationSettings.FileExtensions, progressHandler);

            _messenger.Send(stopProgressBarMessage);

            _libraryViewModel.OnLibraryUpdated(_library);
        }

        private void OnSaveLibrary()
        {
            _libraryService.SaveLibrary(_library);
        }
    }
}
