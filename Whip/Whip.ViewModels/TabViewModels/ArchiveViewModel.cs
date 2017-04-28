using GalaSoft.MvvmLight.Messaging;
using System;
using System.Collections.Generic;
using Whip.Common;
using Whip.Common.Model;
using Whip.Common.TagModel;
using Whip.Common.Utilities;
using Whip.Services.Interfaces;
using Whip.ViewModels.Messages;
using Whip.ViewModels.Utilities;
using Whip.ViewModels.Windows;

namespace Whip.ViewModels.TabViewModels
{
    public class ArchiveViewModel : TabViewModelBase
    {
        private readonly IArchiveService _archiveService;
        private readonly IMessenger _messenger;

        private List<BasicTrackId3Data> _archivedTracks;

        public ArchiveViewModel(IArchiveService archiveService, IMessenger messenger)
            :base(TabType.Archive, IconType.Archive, "Archive")
        {
            _archiveService = archiveService;
            _messenger = messenger;
        }

        public List<BasicTrackId3Data> ArchivedTracks
        {
            get { return _archivedTracks; }
            set { Set(ref _archivedTracks, value); }
        }

        public override void OnShow(Track track)
        {
            var progressBarViewModel = new ProgressBarViewModel(_messenger, "Populating Archive", true);
            var startProgressBarMessage = new ShowDialogMessage(progressBarViewModel);
            
            GetTracks(progressBarViewModel);

            _messenger.Send(startProgressBarMessage);
        }

        private async void GetTracks(ProgressBarViewModel progressBarViewModel)
        {
            var progressHandler = new Progress<ProgressArgs>(progressBarViewModel.Update);
            var stopProgressBarMessage = new HideDialogMessage(progressBarViewModel.Guid);

            ArchivedTracks = await _archiveService.GetArchivedTracksAsync(progressHandler);

            _messenger.Send(stopProgressBarMessage);
        }

        public override void OnHide()
        {
            ArchivedTracks = null;

            base.OnHide();
        }
    }
}
