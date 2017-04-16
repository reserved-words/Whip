﻿using System;
using GalaSoft.MvvmLight.Command;
using Whip.Common;
using Whip.Common.Model;
using Whip.ViewModels.Utilities;
using GalaSoft.MvvmLight.Messaging;
using Whip.ViewModels.Messages;

namespace Whip.ViewModels.TabViewModels
{
    public class CurrentTrackViewModel : TabViewModelBase
    {
        private readonly IMessenger _messenger;

        private Track _track;

        public CurrentTrackViewModel(IMessenger messenger)
            :base(TabType.CurrentTrack, IconType.Music, "Current Track")
        {
            _messenger = messenger;

            EditTrackCommand = new RelayCommand(OnEditTrack);
        }

        public RelayCommand EditTrackCommand { get; private set; }

        public Track Track
        {
            get { return _track; }
            set { Set(ref _track, value); }
        }

        public override void OnCurrentTrackChanged(Track track)
        {
            if (Track == track)
                return;

            Track = track;
        }

        private void OnEditTrack()
        {
            _messenger.Send(new EditTrackMessage(Track));
        }
    }
}