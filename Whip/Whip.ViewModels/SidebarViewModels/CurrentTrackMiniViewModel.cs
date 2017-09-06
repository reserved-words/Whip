using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Messaging;
using Whip.Common.Model;
using Whip.Common.Singletons;
using Whip.Services.Interfaces;
using Whip.ViewModels.Messages;

namespace Whip.ViewModels
{
    public class CurrentTrackMiniViewModel : ViewModelBase
    {
        private readonly IMessenger _messenger;
        private readonly ITrackLoveService _trackLoveService;
        
        private bool _loved;
        private Track _track;

        public CurrentTrackMiniViewModel(ITrackLoveService trackLoveService, IMessenger messenger, Library library, 
            TrackContextMenuViewModel trackContextMenu)
        {
            _messenger = messenger;
            _trackLoveService = trackLoveService;

            TrackContextMenu = trackContextMenu;

            library.Updated += OnLibraryUpdated;

            LoveTrackCommand = new RelayCommand(OnLoveTrack);
            UnloveTrackCommand = new RelayCommand(OnUnloveTrack);
            SetContextMenuTrackCommand = new RelayCommand(OnSetContextMenuTrack);
        }

        public TrackContextMenuViewModel TrackContextMenu { get; }

        public RelayCommand SetContextMenuTrackCommand { get; }
        public RelayCommand LoveTrackCommand { get; }
        public RelayCommand UnloveTrackCommand { get; }

        public Track Track
        {
            get { return _track; }
            set { Set(ref _track, value); }
        }

        public bool Loved
        {
            get { return _loved; }
            set { Set(ref _loved, value); }
        }
        private void OnLibraryUpdated(Track track)
        {
            if (track != null && Track == track)
            {
                RaisePropertyChanged(nameof(Track));
            }
        }

        public async void OnCurrentTrackChanged(Track track)
        {
            Track = track;
            Loved = track != null && await _trackLoveService.IsLovedAsync(track);
        }

        private async void OnLoveTrack()
        {
            Loved = true;

            if (await _trackLoveService.LoveTrackAsync(Track))
                return;

            _messenger.Send(new ShowDialogMessage(_messenger, MessageType.Error, "Error", "There was an error setting this track as Loved"));
            Loved = false;
        }

        private async void OnUnloveTrack()
        {
            Loved = false;

            if (await _trackLoveService.UnloveTrackAsync(Track))
                return;

            _messenger.Send(new ShowDialogMessage(_messenger, MessageType.Error, "Error", "There was an error removing Loved status from this track"));
            Loved = true;
        }

        private void OnSetContextMenuTrack()
        {
            TrackContextMenu.SetTrack(_track);
        }
    }
}
