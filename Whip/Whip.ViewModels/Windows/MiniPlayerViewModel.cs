using GalaSoft.MvvmLight.Messaging;
using Whip.ViewModels.Messages;

namespace Whip.ViewModels.Windows
{
    public class MiniPlayerViewModel
    {
        private readonly IMessenger _messenger;

        public MiniPlayerViewModel(IMessenger messenger, CurrentTrackMiniViewModel currentTrackViewModel, PlayerControlsViewModel playerControlsViewModel)
        {
            _messenger = messenger;

            CurrentTrackMiniViewModel = currentTrackViewModel;
            PlayerControlsViewModel = playerControlsViewModel;
        }

        public CurrentTrackMiniViewModel CurrentTrackMiniViewModel { get; }
        public PlayerControlsViewModel PlayerControlsViewModel { get; }

        public void OnReturnToMainPlayer()
        {
            CurrentTrackMiniViewModel.IsMiniPlayerOpen = false;
            _messenger.Send(new HideMiniPlayerMessage());
        }
    }
}
