using LastFmApi.Interfaces;
using System;
using System.Threading.Tasks;
using Whip.Common.Model;
using Whip.Services.Interfaces;

namespace Whip.LastFm
{
    public class ScrobblingService : IScrobblingService
    {
        private readonly ILastFmApiClientService _clientService;
        private readonly ILastFmScrobblingService _service;

        public ScrobblingService(ILastFmScrobblingService service, ILastFmApiClientService clientService)
        {
            _clientService = clientService;
            _service = service;
        }

        public async Task ScrobbleAsync(Track track, DateTime timePlayed)
        {
            await _service.ScrobbleAsync(_clientService.AuthorizedApiClient, GetTrack(track), timePlayed);
        }

        public async Task UpdateNowPlayingAsync(Track track, int duration)
        {
            await _service.UpdateNowPlayingAsync(_clientService.AuthorizedApiClient, GetTrack(track), duration);
        }

        private LastFmApi.Track GetTrack(Track track)
        {
            return new LastFmApi.Track(track.Title, track.Artist.Name, track.Disc.Album.Title, track.Disc.Album.Artist.Name);
        }
    }
}
