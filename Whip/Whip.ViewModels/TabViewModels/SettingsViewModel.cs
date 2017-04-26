using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Messaging;
using Whip.Common;
using Whip.Services.Interfaces;
using Whip.ViewModels.Utilities;

namespace Whip.ViewModels.TabViewModels
{
    public class SettingsViewModel : EditableTabViewModelBase
    {
        private readonly IFolderDialogService _folderDialogService;
        private readonly IMessenger _messenger;
        private readonly IUserSettings _userSettings;
        
        private string _lastFmUsername;
        private string _lastFmApiKey;
        private string _lastFmApiSecret;
        private string _musicDirectory;
        private string _mainColourRgb;
        private bool _scrobbling;
        private bool _shuffleOn;

        public SettingsViewModel(IUserSettings userSettings, IMessenger messenger, IFolderDialogService folderDialogService)
            :base(TabType.Settings, IconType.Cog, "Settings", false) 
        {
            _folderDialogService = folderDialogService;
            _messenger = messenger;
            _userSettings = userSettings;

            SetMusicDirectoryCommand = new RelayCommand(OnSetMusicDirectory);

            Reset();
        }

        public RelayCommand SetMusicDirectoryCommand { get; private set; }

        public string LastFmUsername
        {
            get { return _lastFmUsername; }
            set { SetModified(nameof(LastFmUsername), ref _lastFmUsername, value); }
        }

        public string LastFmApiKey
        {
            get { return _lastFmApiKey; }
            set { SetModified(nameof(LastFmApiKey), ref _lastFmApiKey, value); }
        }

        public string LastFmApiSecret
        {
            get { return _lastFmApiSecret; }
            set { SetModified(nameof(LastFmApiSecret), ref _lastFmApiSecret, value); }
        }

        public string MainColourRgb
        {
            get { return _mainColourRgb; }
            set { SetModified(nameof(MainColourRgb), ref _mainColourRgb, value); }
        }

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

        public void Reset()
        {
            LastFmUsername = _userSettings.LastFmUsername;
            LastFmApiKey = _userSettings.LastFmApiKey;
            LastFmApiSecret = _userSettings.LastFmApiSecret;
            MusicDirectory = _userSettings.MusicDirectory;
            MainColourRgb = _userSettings.MainColourRgb;
            Scrobbling = _userSettings.Scrobbling;
            ShuffleOn = _userSettings.ShuffleOn;

            Modified = false;
        }

        protected override bool CustomSave()
        {
            _userSettings.LastFmUsername = LastFmUsername;
            _userSettings.LastFmApiKey = LastFmApiKey;
            _userSettings.LastFmApiSecret = LastFmApiSecret;
            _userSettings.MusicDirectory = MusicDirectory;
            _userSettings.MainColourRgb = MainColourRgb;
            _userSettings.Scrobbling = Scrobbling;
            _userSettings.ShuffleOn = ShuffleOn;

            _userSettings.Save();

            return true;
        }

        protected override bool CustomCancel()
        {
            Reset();

            return true;
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
