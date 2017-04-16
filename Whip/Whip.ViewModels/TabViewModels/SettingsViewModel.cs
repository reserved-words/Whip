using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Messaging;
using Whip.Common;
using Whip.Services.Interfaces;
using Whip.ViewModels.Messages;
using Whip.ViewModels.Utilities;

namespace Whip.ViewModels.TabViewModels
{
    public class SettingsViewModel : EditableTabViewModelBase
    {
        private readonly IUserSettingsService _userSettingsService;
        private readonly IMessenger _messenger;

        private string _lastFmUsername;
        private string _lastFmApiKey;
        private string _lastFmApiSecret;
        private string _musicDirectory;
        private string _mainColourRgb;
        
        public SettingsViewModel(IUserSettingsService userSettingsService, IMessenger messenger)
            :base(TabType.Settings, IconType.Cog, "Settings") 
        {
            _userSettingsService = userSettingsService;
            _messenger = messenger;

            MusicDirectory = _userSettingsService.MusicDirectory;
            LastFmApiKey = _userSettingsService.LastFmApiKey;
            LastFmApiSecret = _userSettingsService.LastFmApiSecret;
            LastFmUsername = _userSettingsService.LastFmUsername;

            Modified = false;
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

        public override void OnSave()
        {
            var updateLibrary = MusicDirectory != _userSettingsService.MusicDirectory && !string.IsNullOrEmpty(MusicDirectory);
            var updateLastFmSessionKey = LastFmUsername != _userSettingsService.LastFmUsername;

            _userSettingsService.LastFmUsername = LastFmUsername;
            _userSettingsService.LastFmApiKey = LastFmApiKey;
            _userSettingsService.LastFmApiSecret = LastFmApiSecret;
            _userSettingsService.MusicDirectory = MusicDirectory;
            _userSettingsService.MainColourRgb = MainColourRgb;
            _userSettingsService.Save();
            
            if (updateLibrary)
            {
                _messenger.Send(new LibraryUpdateRequest());
            }

            if (updateLastFmSessionKey)
            {
                // Sort session key
            }
        }

        public override void OnCancel()
        {
            LastFmUsername = _userSettingsService.LastFmUsername;
            LastFmApiKey = _userSettingsService.LastFmApiKey;
            LastFmApiSecret = _userSettingsService.LastFmApiSecret;
            MusicDirectory = _userSettingsService.MusicDirectory;
            MainColourRgb = _userSettingsService.MainColourRgb;
        }
    }
}
