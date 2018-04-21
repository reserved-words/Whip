using System.Collections.ObjectModel;
using System.Linq;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using Whip.Common;
using Whip.Services.Interfaces.Singletons;
using Whip.Services.Interfaces;

namespace Whip.ViewModels
{
    public class PlayShortcutsViewModel : ViewModelBase
    {
        public enum PlaylistType { Quick, Criteria, Ordered }

        public class PlaylistShortcut
        {
            public PlaylistShortcut(int id, PlaylistType type, string title)
            {
                Id = id;
                Type = type;
                Title = title;
            }

            public int Id { get; }
            public PlaylistType Type { get; }
            public string Title { get; }
        }

        private readonly IPlayRequestHandler _playRequestHandler;
        private readonly IPlaylistRepository _repository;
        private readonly ITrackSearchService _trackSearchService;

        public PlayShortcutsViewModel(IPlayRequestHandler playRequestHandler, IPlaylistRepository repository, ITrackSearchService trackSearchService)
        {
            _repository = repository;
            _playRequestHandler = playRequestHandler;
            _trackSearchService = trackSearchService;

            PlayCommand = new RelayCommand<PlaylistShortcut>(OnPlay);
            ShuffleLibraryCommand = new RelayCommand(OnShuffleLibrary);

            Playlists = new ObservableCollection<PlaylistShortcut>();
        }

        public ObservableCollection<PlaylistShortcut> Playlists { get; set; }

        public RelayCommand<PlaylistShortcut> PlayCommand { get; }
        public RelayCommand ShuffleLibraryCommand { get; }

        public void LoadPlaylists()
        {
            var playlists = _repository.GetPlaylists(true);
            Playlists.Clear();
            var criteriaPlaylists = playlists.CriteriaPlaylists.Select(p => new PlaylistShortcut(p.Id, PlaylistType.Criteria, p.Title));
            var orderedPlaylists = playlists.OrderedPlaylists.Select(p => new PlaylistShortcut(p.Id, PlaylistType.Ordered, p.Title));
            var combined = criteriaPlaylists.Concat(orderedPlaylists).OrderBy(p => p.Title).ToList();
            combined.ForEach(Playlists.Add);
        }

        private void OnPlay(PlaylistShortcut playlist)
        {
            switch (playlist.Type)
            {
                case PlaylistType.Criteria:
                    var criteriaPlaylist = _repository.GetCriteriaPlaylist(playlist.Id);
                    _playRequestHandler.PlayCriteriaPlaylist(criteriaPlaylist.Title, _trackSearchService.GetTracks(criteriaPlaylist));
                    break;
                case PlaylistType.Ordered:
                    var orderedPlaylist = _repository.GetOrderedPlaylist(playlist.Id);
                    _playRequestHandler.PlayOrderedPlaylist(orderedPlaylist.Title, _trackSearchService.GetTracks(orderedPlaylist.Tracks));
                    break;
                case PlaylistType.Quick:

                    break;
            }
        }

        private void OnShuffleLibrary()
        {
            _playRequestHandler.PlayAll(SortType.Random);
        }
    }
}
