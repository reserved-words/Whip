using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Messaging;
using System;
using Whip.Services.Interfaces;
using Whip.ViewModels.Messages;

namespace Whip.ViewModels.Windows
{
    public class ApplicationSettingsViewModel : DialogViewModel
    {
        private readonly IMessenger _messenger;
        private readonly IUserSettingsService _userSettingsService;

        private string _musicDirectory;

        public ApplicationSettingsViewModel(IUserSettingsService userSettingsService, IMessenger messenger) 
            : base("Application Settings")
        {
            _userSettingsService = userSettingsService;
            _messenger = messenger;

            MusicDirectory = _userSettingsService.MusicDirectory;

            CancelCommand = new RelayCommand(OnCancel);
            SaveCommand = new RelayCommand(OnSave);
        }
        
        public string MusicDirectory
        {
            get { return _musicDirectory; }
            set { Set(ref _musicDirectory, value); }
        }

        public RelayCommand CancelCommand { get; private set; }
        public RelayCommand SaveCommand { get; private set; }

        private void OnSave()
        {
            _userSettingsService.MusicDirectory = MusicDirectory;
            _userSettingsService.Save();
            Close();
        }

        private void OnCancel()
        {
            Close();
        }

        private void Close()
        {
            _messenger.Send(new HideDialogMessage(Guid));
        }
    }
}
