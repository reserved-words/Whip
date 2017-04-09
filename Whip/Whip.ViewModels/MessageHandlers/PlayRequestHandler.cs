using GalaSoft.MvvmLight.Messaging;
using Whip.Common.Interfaces;
using Whip.Common.Singletons;
using Whip.Services.Interfaces;
using Whip.ViewModels.Messages;

namespace Whip.ViewModels.MessageHandlers
{
    public class PlayRequestHandler : IStartable
    {
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
            _messenger.Register<PlayArtistsMessage>(this, OnPlayArtists);
            _messenger.Register<PlayAlbumsMessage>(this, OnPlayAlbums);
            _messenger.Register<PlayGroupingMessage>(this, OnPlayGrouping);
            _messenger.Register<PlayMessage>(this, OnPlayAll);
        }

        public void Stop()
        {
            _messenger.Unregister<PlayArtistsMessage>(this, OnPlayArtists);
            _messenger.Unregister<PlayAlbumsMessage>(this, OnPlayAlbums);
            _messenger.Unregister<PlayGroupingMessage>(this, OnPlayGrouping);
            _messenger.Unregister<PlayMessage>(this, OnPlayAll);
        }

        private void OnPlayAll(PlayMessage message)
        {
            _playlist.Set(_trackFilterService.GetAll(message.SortType), message.StartAt);
        }

        private void OnPlayGrouping(PlayGroupingMessage message)
        {
            _playlist.Set(_trackFilterService.GetTracksByGrouping(message.Grouping, message.SortType), message.StartAt);
        }

        private void OnPlayAlbums(PlayAlbumsMessage message)
        {
            _playlist.Set(_trackFilterService.GetTracksFromAlbums(message.Albums, message.SortType), message.StartAt);
        }

        private void OnPlayArtists(PlayArtistsMessage message)
        {
            _playlist.Set(_trackFilterService.GetTracksByArtists(message.Artists, message.SortType), message.StartAt);
        }
    }
}
