using GalaSoft.MvvmLight.Command;
using Whip.Services.Interfaces;
using Whip.ViewModels.Singletons;
using Whip.ViewModels.Singletons.Interfaces;

namespace Whip.ViewModels
{
    public class MainWindowViewModel
    {
        private readonly ILibraryService _libraryService;
        private readonly IUserSettingsService _userSettingsService;
        private readonly ILibrary _library;

        public MainWindowViewModel(ILibraryService libraryService, IUserSettingsService userSettingsService, ILibrary library)
        {
            _libraryService = libraryService;
            _userSettingsService = userSettingsService;
            _library = library;

            PopulateLibraryCommand = new RelayCommand(OnPopulateLibrary);
            SaveLibraryCommand = new RelayCommand(OnSaveLibrary);
        }
        
        public MainViewModel MainViewModel => new MainViewModel();
        public SidebarViewModel SidebarViewModel => new SidebarViewModel();

        public RelayCommand PopulateLibraryCommand { get; private set; }
        public RelayCommand SaveLibraryCommand { get; private set; }

        private void OnPopulateLibrary()
        {
            _library.Artists = _libraryService.GetLibrary(_userSettingsService.MusicDirectory, ApplicationSettings.FileExtensions);
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
