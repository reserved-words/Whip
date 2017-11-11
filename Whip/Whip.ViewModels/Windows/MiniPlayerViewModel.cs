
namespace Whip.ViewModels.Windows
{
    public class MiniPlayerViewModel
    {
        public MiniPlayerViewModel(CurrentTrackMiniViewModel currentTrackViewModel, PlayerControlsViewModel playerControlsViewModel) 
        {
            CurrentTrackMiniViewModel = currentTrackViewModel;
            PlayerControlsViewModel = playerControlsViewModel;
        }

        public CurrentTrackMiniViewModel CurrentTrackMiniViewModel { get; }
        public PlayerControlsViewModel PlayerControlsViewModel { get; }
    }
}
