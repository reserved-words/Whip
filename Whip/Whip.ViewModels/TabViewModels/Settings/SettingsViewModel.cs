using GalaSoft.MvvmLight.Command;
using System.ComponentModel.DataAnnotations;
using Whip.Services.Interfaces;
using Whip.ViewModels.Utilities;
using static Whip.Common.Resources;

namespace Whip.ViewModels.TabViewModels.Settings
{
    public class SettingsViewModel : EditableViewModelBase
    {
        private readonly IFolderDialogService _folderDialogService;
        private readonly IUserSettings _userSettings;

        private string _archiveDirectory;
        private string _lastFmUsername;
        private string _musicDirectory;
        private string _mainColourRgb;
        private bool _scrobbling;
        private bool _shuffleOn;

        public SettingsViewModel(IFolderDialogService folderDialogService, IUserSettings userSettings)
        {
            SetMusicDirectoryCommand = new RelayCommand(OnSetMusicDirectory);
            SetArchiveDirectoryCommand = new RelayCommand(OnSetArchiveDirectory);

            _folderDialogService = folderDialogService;
            _userSettings = userSettings;
        }

        public RelayCommand SetMusicDirectoryCommand { get; private set; }
        public RelayCommand SetArchiveDirectoryCommand { get; private set; }

        [Display(Name = "Archive Directory")]
        public string ArchiveDirectory
        {
            get { return _archiveDirectory; }
            set { SetModified(nameof(ArchiveDirectory), ref _archiveDirectory, value); }
        }

        [Required(ErrorMessageResourceName = nameof(RequiredErrorMessage), ErrorMessageResourceType = typeof(Common.Resources))]
        [Display(Name = "Last.FM Username")]
        public string LastFmUsername
        {
            get { return _lastFmUsername; }
            set { SetModified(nameof(LastFmUsername), ref _lastFmUsername, value); }
        }

        [Required(ErrorMessageResourceName = nameof(RequiredErrorMessage), ErrorMessageResourceType = typeof(Common.Resources))]
        [Display(Name = "Main Colour")]
        public string MainColourRgb
        {
            get { return _mainColourRgb; }
            set { SetModified(nameof(MainColourRgb), ref _mainColourRgb, value); }
        }

        [Required(ErrorMessageResourceName = nameof(RequiredErrorMessage), ErrorMessageResourceType = typeof(Common.Resources))]
        [Display(Name = "Music Directory")]
        public string MusicDirectory
        {
            get { return _musicDirectory; }
            set { SetModified(nameof(MusicDirectory), ref _musicDirectory, value); }
        }

        public bool Scrobbling
        {
            get { return _scrobbling; }
            set { SetModified(nameof(Scrobbling), ref _scrobbling, value); }
        }

        public bool ShuffleOn
        {
            get { return _shuffleOn; }
            set { SetModified(nameof(ShuffleOn), ref _shuffleOn, value); }
        }

        public string InternetStatus => _userSettings.Offline ? "Offline" : "Online";

        public string InternetStatusDetails => _userSettings.Offline
            ? OfflineErrorMessageDetails
            : "";

        public bool LastFmOn => _userSettings.LastFmStatus;

        public string LastFmStatus => _userSettings.LastFmStatus ? "On" : "Off";

        public string LastFmStatusDetails => _userSettings.LastFmStatus
            ? ""
            : LastFmOffErrorMessageDetails;

        public string LastFmErrorMessage => _userSettings.LastFmErrorMessage;

        public void Reset()
        {
            ArchiveDirectory = _userSettings.ArchiveDirectory;
            LastFmUsername = _userSettings.LastFmUsername;
            MusicDirectory = _userSettings.MusicDirectory;
            MainColourRgb = _userSettings.MainColourRgb;
            Scrobbling = _userSettings.Scrobbling;
            ShuffleOn = _userSettings.ShuffleOn;

            Modified = false;
        }

        public void Update()
        {
            _userSettings.ArchiveDirectory = ArchiveDirectory;
            _userSettings.LastFmUsername = LastFmUsername;
            _userSettings.MusicDirectory = MusicDirectory;
            _userSettings.MainColourRgb = MainColourRgb;
            _userSettings.Scrobbling = Scrobbling;
            _userSettings.ShuffleOn = ShuffleOn;

            _userSettings.SaveAsync();
        }

        private void OnSetArchiveDirectory()
        {
            var selectedDirectory = _folderDialogService.OpenFolderDialog(ArchiveDirectory);
            if (selectedDirectory != null)
            {
                ArchiveDirectory = selectedDirectory;
            }
        }

        private void OnSetMusicDirectory()
        {
            var selectedDirectory = _folderDialogService.OpenFolderDialog(MusicDirectory);
            if (selectedDirectory != null)
            {
                MusicDirectory = selectedDirectory;
            }
        }
    }
}
