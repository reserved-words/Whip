using GalaSoft.MvvmLight.Messaging;
using Whip.Common.Interfaces;
using Whip.Common.Singletons;
using Whip.Services.Interfaces;
using Whip.ViewModels.Messages;

namespace Whip.ViewModels.MessageHandlers
{
    public class PlayRequestHandler : IStartable
    {
        private const string LibraryPlaylistName = "Library";

        private readonly IMessenger _messenger;
        private readonly Playlist _playlist;
        private readonly ITrackFilterService _trackFilterService;

        public PlayRequestHandler(IMessenger messenger, Playlist playlist, ITrackFilterService trackFilterService)
        {
            _messenger = messenger;
            _playlist = playlist;
            _trackFilterService = trackFilterService;
        }

        public void Start()
        {
            _messenger.Register<PlayArtistMessage>(this, OnPlayArtist);
            _messenger.Register<PlayAlbumMessage>(this, OnPlayAlbum);
            _messenger.Register<PlayGroupingMessage>(this, OnPlayGrouping);
            _messenger.Register<PlayAllMessage>(this, OnPlayAll);
            _messenger.Register<MoveToTrackMessage>(this, OnMoveToTrack);
        }

        public void Stop()
        {
            _messenger.Unregister<PlayArtistMessage>(this, OnPlayArtist);
            _messenger.Unregister<PlayAlbumMessage>(this, OnPlayAlbum);
            _messenger.Unregister<PlayGroupingMessage>(this, OnPlayGrouping);
            _messenger.Unregister<PlayAllMessage>(this, OnPlayAll);
            _messenger.Unregister<MoveToTrackMessage>(this, OnMoveToTrack);
        }

        private void OnMoveToTrack(MoveToTrackMessage message)
        {
            _playlist.Set(_playlist.PlaylistName, _playlist.Tracks, message.Track);
        }

        private void OnPlayAll(PlayAllMessage message)
        {
            _playlist.Set(LibraryPlaylistName, _trackFilterService.GetAll(message.SortType), message.StartAt);
        }

        private void OnPlayGrouping(PlayGroupingMessage message)
        {
            _playlist.Set(message.Grouping, _trackFilterService.GetTracksByGrouping(message.Grouping, message.SortType), message.StartAt);
        }

        private void OnPlayAlbum(PlayAlbumMessage message)
        {
            _playlist.Set(message.Album.ToString(), _trackFilterService.GetTracksFromAlbum(message.Album, message.SortType), message.StartAt);
        }

        private void OnPlayArtist(PlayArtistMessage message)
        {
            _playlist.Set(message.Artist.Name, _trackFilterService.GetTracksByArtist(message.Artist, message.SortType), message.StartAt);
        }
    }
}
