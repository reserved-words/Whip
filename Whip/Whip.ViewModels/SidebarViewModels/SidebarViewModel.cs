﻿using GalaSoft.MvvmLight;
using Whip.Common.Interfaces;
using Whip.Common.Model;
using Whip.ViewModels.MessageHandlers;

namespace Whip.ViewModels
{
    public class SidebarViewModel : ViewModelBase
    {
        public SidebarViewModel(CurrentTrackMiniViewModel currentTrackMiniViewModel, PlayerControlsViewModel playerControlsViewModel,
            IPlayerUpdate playerUpdate)
        {
            CurrentTrackMiniViewModel = currentTrackMiniViewModel;
            PlayerControlsViewModel = playerControlsViewModel;

            playerUpdate.NewTrackStarted += OnNewTrackStarted;
        }

        public CurrentTrackMiniViewModel CurrentTrackMiniViewModel { get; private set; }
        public PlayerControlsViewModel PlayerControlsViewModel { get; private set; }

        public void OnCurrentTrackChanged(Track track)
        {
            CurrentTrackMiniViewModel.OnCurrentTrackChanged(track);
            PlayerControlsViewModel.OnCurrentTrackChanged(track);
        }

        private void OnNewTrackStarted(Track track)
        {
            PlayerControlsViewModel.OnNewTrackStarted(track);
            CurrentTrackMiniViewModel.OnNewTrackStarted(track);
        }
    }
}
