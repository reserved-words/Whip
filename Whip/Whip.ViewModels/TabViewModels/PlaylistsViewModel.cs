using GalaSoft.MvvmLight.Messaging;
using Whip.Common;
using Whip.Common.Model;
using Whip.Services.Interfaces;
using Whip.ViewModels.TabViewModels.Playlists;
using Whip.ViewModels.Utilities;

namespace Whip.ViewModels.TabViewModels
{
    public class PlaylistsViewModel : TabViewModelBase
    {
        private readonly IPlaylistRepository _repository;

        private bool _populated;

        public PlaylistsViewModel(IPlaylistRepository repository, Common.Singletons.Library library, IMessenger messenger, IPlaylistService playlistService)
            :base(TabType.Playlists, IconType.ListUl, "Playlists")
        {
            _repository = repository;

            OrderedPlaylists = new OrderedPlaylistsViewModel();
            CriteriaPlaylists = new CriteriaPlaylistsViewModel(library, messenger, playlistService);
            StandardPlaylists = new StandardPlaylistsViewModel(library, messenger);
        }

        public StandardPlaylistsViewModel StandardPlaylists { get; private set; }
        public OrderedPlaylistsViewModel OrderedPlaylists { get; private set; }
        public CriteriaPlaylistsViewModel CriteriaPlaylists { get; private set; }

        public override void OnShow(Track currentTrack)
        {
            StandardPlaylists.UpdateOptions();

            if (_populated)
                return;

            _populated = true;

            var playlists = _repository.GetPlaylists();

            OrderedPlaylists.Update(playlists.OrderedPlaylists);
            CriteriaPlaylists.Update(playlists.CriteriaPlaylists);
        }
    }
}
