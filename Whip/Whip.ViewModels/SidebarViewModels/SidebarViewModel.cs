using GalaSoft.MvvmLight;
using Whip.Common.Model;
using Whip.ViewModels.SidebarViewModels;

namespace Whip.ViewModels
{
    public class SidebarViewModel : ViewModelBase
    {
        public SidebarViewModel(CurrentTrackMiniViewModel currentTrackMiniViewModel, PlayerControlsViewModel playerControlsViewModel,
            SettingsIconsViewModel settingsIconsViewModel, PlayShortcutsViewModel playShortcutsViewModel)
        {
            CurrentTrackMiniViewModel = currentTrackMiniViewModel;
            PlayerControlsViewModel = playerControlsViewModel;
            SettingsIconsViewModel = settingsIconsViewModel;
            PlayShortcutsViewModel = playShortcutsViewModel;
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
