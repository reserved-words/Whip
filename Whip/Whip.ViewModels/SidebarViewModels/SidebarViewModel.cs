using GalaSoft.MvvmLight;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Whip.Common.Model;

namespace Whip.ViewModels
{
    public class SidebarViewModel : ViewModelBase
    {
        public SidebarViewModel(CurrentTrackMiniViewModel currentTrackMiniViewModel, PlayerControlsViewModel playerControlsViewModel)
        {
            CurrentTrackMiniViewModel = currentTrackMiniViewModel;
            PlayerControlsViewModel = playerControlsViewModel;
        }

        public CurrentTrackMiniViewModel CurrentTrackMiniViewModel { get; private set; }
        public PlayerControlsViewModel PlayerControlsViewModel { get; private set; }

        public void OnCurrentTrackChanged(Track track)
        {
            CurrentTrackMiniViewModel.OnCurrentTrackChanged(track);
            PlayerControlsViewModel.OnCurrentTrackChanged(track);
        }
    }
}
