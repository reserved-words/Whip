﻿using GalaSoft.MvvmLight.Messaging;
using Whip.Common;
using Whip.Common.Model;
using Whip.Services.Interfaces;
using Whip.ViewModels.Messages;
using Whip.ViewModels.TabViewModels.Playlists;
using Whip.ViewModels.Utilities;
using Whip.ViewModels.Windows;
using static Whip.ViewModels.Windows.ConfirmationViewModel;

namespace Whip.ViewModels.TabViewModels
{
    public class PlaylistsViewModel : TabViewModelBase
    {
        private readonly IMessenger _messenger;
        private readonly IPlaylistRepository _repository;

        public PlaylistsViewModel(IPlaylistRepository repository, Common.Singletons.Library library, IMessenger messenger, ITrackSearchService trackSearchService)
            :base(TabType.Playlists, IconType.ListUl, "Playlists")
        {
            _messenger = messenger;
            _repository = repository;

            OrderedPlaylists = new OrderedPlaylistsViewModel(this, _messenger, trackSearchService, _repository);
            CriteriaPlaylists = new CriteriaPlaylistsViewModel(this, library, messenger, trackSearchService);
            StandardPlaylists = new StandardPlaylistsViewModel(library, messenger);
        }

        public StandardPlaylistsViewModel StandardPlaylists { get; private set; }
        public OrderedPlaylistsViewModel OrderedPlaylists { get; private set; }
        public CriteriaPlaylistsViewModel CriteriaPlaylists { get; private set; }

        public override void OnShow(Track currentTrack)
        {
            StandardPlaylists.UpdateOptions();

            var playlists = _repository.GetPlaylists();

            OrderedPlaylists.Update(playlists.OrderedPlaylists);
            CriteriaPlaylists.Update(playlists.CriteriaPlaylists);
        }

        public void Remove(CriteriaPlaylist playlist)
        {
            var confirmation = new ConfirmationViewModel(_messenger, "Delete Playlist Confirmation", $"Are you sure you want to delete {playlist.Title}?", 
                ConfirmationType.YesNo, false);

            _messenger.Send(new ShowDialogMessage(confirmation));

            if (!confirmation.Result)
                return;

            _repository.Delete(playlist);
            OnShow(null);
        }

        public void Remove(OrderedPlaylist playlist)
        {
            var confirmation = new ConfirmationViewModel(_messenger, "Delete Playlist Confirmation", $"Are you sure you want to delete {playlist.Title}?",
                ConfirmationType.YesNo, false);

            _messenger.Send(new ShowDialogMessage(confirmation));

            if (!confirmation.Result)
                return;

            _repository.Delete(playlist);
            OnShow(null);
        }
    }
}
