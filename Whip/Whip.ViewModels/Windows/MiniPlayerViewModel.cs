using GalaSoft.MvvmLight.Messaging;
using Whip.Common;

namespace Whip.ViewModels.Windows
{
    public class MiniPlayerViewModel
    {
        public MiniPlayerViewModel(CurrentTrackMiniViewModel currentTrackViewModel) 
        {
            CurrentTrackMiniViewModel = currentTrackViewModel;
        }

        public CurrentTrackMiniViewModel CurrentTrackMiniViewModel { get; }
    }
}
