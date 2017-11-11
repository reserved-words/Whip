using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Messaging;
using Whip.Common.Model;
using Whip.ViewModels.Messages;
using Whip.ViewModels.Windows;

namespace Whip.ViewModels
{
    public class SidebarViewModel : ViewModelBase
    {
        private readonly IMessenger _messenger;

        public SidebarViewModel(IMessenger messenger, CurrentTrackMiniViewModel currentTrackMiniViewModel, PlayerControlsViewModel playerControlsViewModel,
            SettingsIconsViewModel settingsIconsViewModel, PlayShortcutsViewModel playShortcutsViewModel)
        {
            _messenger = messenger;

            CurrentTrackMiniViewModel = currentTrackMiniViewModel;
            PlayerControlsViewModel = playerControlsViewModel;
            SettingsIconsViewModel = settingsIconsViewModel;
            PlayShortcutsViewModel = playShortcutsViewModel;

            CurrentTrackMiniViewModel.OpenMiniPlayer += OnOpenMiniPlayer;
        }

        private void OnOpenMiniPlayer()
        {
            var miniPlayer = new MiniPlayerViewModel(_messenger, CurrentTrackMiniViewModel, PlayerControlsViewModel);
            _messenger.Send(new ShowMiniPlayerMessage(miniPlayer));
        }

        public CurrentTrackMiniViewModel CurrentTrackMiniViewModel { get; }
        public PlayerControlsViewModel PlayerControlsViewModel { get; }
        public PlayShortcutsViewModel PlayShortcutsViewModel { get; }
        public SettingsIconsViewModel SettingsIconsViewModel { get; }

        public void OnCurrentTrackChanged(Track track)
        {
            CurrentTrackMiniViewModel.OnCurrentTrackChanged(track);
            PlayerControlsViewModel.OnCurrentTrackChanged(track);
        }
    }
}
