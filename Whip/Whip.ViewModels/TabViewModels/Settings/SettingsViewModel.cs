﻿using GalaSoft.MvvmLight.Command;
using System.ComponentModel.DataAnnotations;
using Whip.Services.Interfaces;
using Whip.ViewModels.Utilities;
using static Whip.Resources.Resources;

namespace Whip.ViewModels.TabViewModels.Settings
{
    public class SettingsViewModel : EditableViewModelBase
    {
        private readonly IFolderDialogService _folderDialogService;
        private readonly IUserSettings _userSettings;

        private string _lastFmUsername;
        private string _lastFmApiKey;
        private string _lastFmApiSecret;
        private string _musicDirectory;
        private string _mainColourRgb;
        private bool _scrobbling;
        private bool _shuffleOn;

        public SettingsViewModel(IFolderDialogService folderDialogService, IUserSettings userSettings)
        {
            SetMusicDirectoryCommand = new RelayCommand(OnSetMusicDirectory);

            _folderDialogService = folderDialogService;
            _userSettings = userSettings;
        }

        public RelayCommand SetMusicDirectoryCommand { get; private set; }

        [Required(ErrorMessageResourceName = nameof(RequiredErrorMessage), ErrorMessageResourceType = typeof(Resources.Resources))]
        [Display(Name = "Last.FM Username")]
        public string LastFmUsername
        {
            get { return _lastFmUsername; }
            set { SetModified(nameof(LastFmUsername), ref _lastFmUsername, value); }
        }

        [Required(ErrorMessageResourceName = nameof(RequiredErrorMessage), ErrorMessageResourceType = typeof(Resources.Resources))]
        [Display(Name = "Last.FM API Key")]
        public string LastFmApiKey
        {
            get { return _lastFmApiKey; }
            set { SetModified(nameof(LastFmApiKey), ref _lastFmApiKey, value); }
        }

        [Required(ErrorMessageResourceName = nameof(RequiredErrorMessage), ErrorMessageResourceType = typeof(Resources.Resources))]
        [Display(Name = "Last.FM API Secret")]
        public string LastFmApiSecret
        {
            get { return _lastFmApiSecret; }
            set { SetModified(nameof(LastFmApiSecret), ref _lastFmApiSecret, value); }
        }

        [Required(ErrorMessageResourceName = nameof(RequiredErrorMessage), ErrorMessageResourceType = typeof(Resources.Resources))]
        [Display(Name = "Main Colour")]
        public string MainColourRgb
        {
            get { return _mainColourRgb; }
            set { SetModified(nameof(MainColourRgb), ref _mainColourRgb, value); }
        }

        [Required(ErrorMessageResourceName = nameof(RequiredErrorMessage), ErrorMessageResourceType = typeof(Resources.Resources))]
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

        public void Update()
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