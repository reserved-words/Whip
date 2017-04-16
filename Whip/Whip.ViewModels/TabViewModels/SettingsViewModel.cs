using GalaSoft.MvvmLight.Messaging;
using Whip.Common;
using Whip.Services.Interfaces;
using Whip.ViewModels.Messages;
using Whip.ViewModels.Utilities;

namespace Whip.ViewModels.TabViewModels
{
    public class SettingsViewModel : EditableTabViewModelBase
    {
        private readonly IUserSettings _userSettings;
        private readonly IMessenger _messenger;

        private string _lastFmUsername;
        private string _lastFmApiKey;
        private string _lastFmApiSecret;
        private string _musicDirectory;
        private string _mainColourRgb;
        private bool _scrobbling;
        private bool _shuffleOn;

        public SettingsViewModel(IUserSettings userSettings, IMessenger messenger)
            :base(TabType.Settings, IconType.Cog, "Settings", false) 
        {
            _userSettings = userSettings;
            _messenger = messenger;

            Reset();
        }

        public string LastFmUsername
        {
            get { return _lastFmUsername; }
            set { SetModified(ref _lastFmUsername, value); }
        }

        public string LastFmApiKey
        {
            get { return _lastFmApiKey; }
            set { SetModified(ref _lastFmApiKey, value); }
        }

        public string LastFmApiSecret
        {
            get { return _lastFmApiSecret; }
            set { SetModified(ref _lastFmApiSecret, value); }
        }

        public string MainColourRgb
        {
            get { return _mainColourRgb; }
            set { SetModified(ref _mainColourRgb, value); }
        }

        public string MusicDirectory
        {
            get { return _musicDirectory; }
            set { SetModified(ref _musicDirectory, value); }
        }

        public bool Scrobbling
        {
            get { return _scrobbling; }
            set { SetModified(ref _scrobbling, value); }
        }
        
        public bool ShuffleOn
        {
            get { return _shuffleOn; }
            set { SetModified(ref _shuffleOn, value); }
        }

        protected override void CustomSave()
        {
            _userSettings.LastFmUsername = LastFmUsername;
            _userSettings.LastFmApiKey = LastFmApiKey;
            _userSettings.LastFmApiSecret = LastFmApiSecret;
            _userSettings.MusicDirectory = MusicDirectory;
            _userSettings.MainColourRgb = MainColourRgb;
            _userSettings.Scrobbling = Scrobbling;
            _userSettings.ShuffleOn = ShuffleOn;

            _userSettings.Save();
        }

        protected override void CustomCancel()
        {
            Reset();
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
    }
}
