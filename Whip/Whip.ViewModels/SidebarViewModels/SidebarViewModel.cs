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

        public SidebarViewModel(CurrentTrackMiniViewModel currentTrackMiniViewModel, PlayerControlsViewModel playerControlsViewModel,
            SettingsIconsViewModel settingsIconsViewModel, PlayShortcutsViewModel playShortcutsViewModel, IMessenger messenger)
        {
            _messenger = messenger;

            CurrentTrackMiniViewModel = currentTrackMiniViewModel;
            PlayerControlsViewModel = playerControlsViewModel;
            SettingsIconsViewModel = settingsIconsViewModel;
            PlayShortcutsViewModel = playShortcutsViewModel;

            SettingsIconsViewModel.OpenMiniPlayer += OnOpenMiniPlayer;
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

        public void Load()
        {
            PlayShortcutsViewModel.Load();
        }

        private void OnOpenMiniPlayer()
        {
            var miniPlayer = new MiniPlayerViewModel(_messenger, CurrentTrackMiniViewModel, PlayerControlsViewModel);
            _messenger.Send(new ShowMiniPlayerMessage(miniPlayer));
        }
    }
}
