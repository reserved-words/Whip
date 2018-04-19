using System;
using GalaSoft.MvvmLight.Messaging;
using Whip.Common;
using Whip.Common.Model;
using Whip.Services.Interfaces;
using Whip.Services.Interfaces.Singletons;
using Whip.ViewModels.Messages;
using Whip.ViewModels.TabViewModels.Playlists;
using Whip.ViewModels.Utilities;
using Whip.ViewModels.Windows;
using static Whip.ViewModels.Windows.ConfirmationViewModel;

namespace Whip.ViewModels.TabViewModels
{
    public class PlaylistsViewModel : TabViewModelBase
    {
        public event Action FavouritePlaylistsUpdated = delegate { };

        private readonly IMessenger _messenger;
        private readonly IPlaylistRepository _repository;

        public PlaylistsViewModel(IPlaylistRepository repository, Common.Singletons.Library library, IMessenger messenger,
            ITrackSearchService trackSearchService, IPlayRequestHandler playRequestHandler)
            :base(TabType.Playlists, IconType.ListUl, "Playlists")
        {
            _messenger = messenger;
            _repository = repository;

            OrderedPlaylists = new OrderedPlaylistsViewModel(this, _messenger, trackSearchService, _repository, playRequestHandler);
            CriteriaPlaylists = new CriteriaPlaylistsViewModel(this, messenger, trackSearchService, _repository, playRequestHandler);
            StandardPlaylists = new StandardPlaylistsViewModel(library, messenger, playRequestHandler);
        }

        public StandardPlaylistsViewModel StandardPlaylists { get; }
        public OrderedPlaylistsViewModel OrderedPlaylists { get; }
        public CriteriaPlaylistsViewModel CriteriaPlaylists { get; }

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
                ConfirmationType.YesNo);

            _messenger.Send(new ShowDialogMessage(confirmation));

            if (!confirmation.Result)
                return;

            _repository.Delete(playlist);
            OnShow(null);
        }

        public void Remove(OrderedPlaylist playlist)
        {
            var confirmation = new ConfirmationViewModel(_messenger, "Delete Playlist Confirmation", $"Are you sure you want to delete {playlist.Title}?",
                ConfirmationType.YesNo);

            _messenger.Send(new ShowDialogMessage(confirmation));

            if (!confirmation.Result)
                return;

            _repository.Delete(playlist);
            OnShow(null);
        }

        public void OnFavouritePlaylistsUpdated()
        {
            FavouritePlaylistsUpdated.Invoke();
        }
    }
}
