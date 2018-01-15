using GalaSoft.MvvmLight.Messaging;
using LastFmApi;
using System;
using System.IO;
using System.Threading.Tasks;
using Whip.LastFm;
using Whip.Services.Interfaces;
using Whip.ViewModels.Messages;
using static Whip.Common.Resources;

namespace Whip.View
{
    public class UserSettings : IUserSettings
    {
        private readonly ILastFmApiClientService _lastFmApiClientService;
        private readonly IMessenger _messenger;

        private bool _scrobblingStatusChanged;
        private bool _shuffleStatusChanged;
        private bool _libraryUpdated;
        private bool _lastFmUsernameChanged;
        
        public UserSettings(IMessenger messenger, ILastFmApiClientService lastFmApiClientService)
        {
            _lastFmApiClientService = lastFmApiClientService;
            _messenger = messenger;
        }

        public event Action ScrobblingStatusChanged;
        public event Action ShufflingStatusChanged;

        public bool EssentialSettingsSet => !string.IsNullOrEmpty(MusicDirectory)
            && !string.IsNullOrEmpty(LastFmUsername)
            && !string.IsNullOrEmpty(MainColourRgb);

        public string MusicDirectory
        {
            get { return Properties.Settings.Default.MusicDirectory; }
            set
            {
                if (value != Properties.Settings.Default.MusicDirectory)
                {
                    Properties.Settings.Default.MusicDirectory = value;
                    _libraryUpdated = true;
                }
            }
        }

        public string ArchiveDirectory
        {
            get { return Properties.Settings.Default.ArchiveDirectory; }
            set { Properties.Settings.Default.ArchiveDirectory = value; }
        }

        public string MainColourRgb
        {
            get { return Properties.Settings.Default.MainColourRgb; }
            set { Properties.Settings.Default.MainColourRgb = value; }
        }

        public string LastFmApiSessionKey
        {
            get { return Properties.Settings.Default.LastFmApiSessionKey; }
            set { Properties.Settings.Default.LastFmApiSessionKey = value; }
        }

        public string LastFmUsername
        {
            get { return Properties.Settings.Default.LastFmUsername; }
            set
            {
                if (value != Properties.Settings.Default.LastFmUsername)
                {
                    Properties.Settings.Default.LastFmUsername = value;
                    Properties.Settings.Default.LastFmApiSessionKey = null;
                    _lastFmUsernameChanged = true;
                }
            }
        }

        public bool Scrobbling
        {
            get { return Properties.Settings.Default.Scrobbling; }
            set
            {
                if (value != Properties.Settings.Default.Scrobbling)
                {
                    Properties.Settings.Default.Scrobbling = value;
                    _scrobblingStatusChanged = true;
                }
            }
        }

        public bool ShuffleOn
        {
            get { return Properties.Settings.Default.ShuffleOn; }
            set
            {
                if (value != Properties.Settings.Default.ShuffleOn)
                {
                    Properties.Settings.Default.ShuffleOn = value;
                    _shuffleStatusChanged = true;
                }
            }
        }

        public async Task SaveAsync()
        {
            if (_lastFmUsernameChanged)
            {
                _lastFmUsernameChanged = false;
                await SetLastFmClients();
            }

            Properties.Settings.Default.Save();

            if (_scrobblingStatusChanged)
            {
                _scrobblingStatusChanged = false;
                ScrobblingStatusChanged?.Invoke();
            }

            if (_shuffleStatusChanged)
            {
                _shuffleStatusChanged = false;
                ShufflingStatusChanged?.Invoke();
            }

            if (_libraryUpdated && !string.IsNullOrEmpty(MusicDirectory))
            {
                _libraryUpdated = false;
                _messenger.Send(new LibraryUpdateRequest());
            }
        }

        private async Task SetLastFmClients()
        {
            if (string.IsNullOrEmpty(LastFmUsername))
                return;

            try
            {
                await _lastFmApiClientService.SetClients(LastFmUsername, LastFmApiSessionKey);
                LastFmApiSessionKey = _lastFmApiClientService.AuthorizedApiClient.SessionKey;
            }
            catch (LastFmApiException ex)
            {
                if (ex.ErrorCode == ErrorCode.UserNotLoggedIn)
                {
                    _messenger.Send(new ShowDialogMessage(_messenger, MessageType.Error, "Last.FM Error", ex.Message));
                    LastFmUsername = string.Empty;
                    LastFmApiSessionKey = null;
                    return;
                }

                throw;
            }
        }

        public async Task SetStartupDefaultsAsync()
        {
            await SetLastFmClients();

            Properties.Settings.Default.Save();
        }

        public string DataDirectory => Path.Combine(MusicDirectory, string.Format("_{0}", ApplicationTitle));
    }
}
