using System;
using System.Threading.Tasks;
using Whip.Common.Model;
using Whip.Services.Interfaces;

namespace Whip.LastFm
{
    public class Scrobbler : IScrobbler
    {
        private readonly ILastFmApiClientService _clientService;
        private readonly LastFmApi.Interfaces.IScrobblingService _service;

        public Scrobbler(LastFmApi.Interfaces.IScrobblingService service, ILastFmApiClientService clientService)
        {
            _clientService = clientService;
            _service = service;
        }

        public async Task<bool> ScrobbleAsync(Track track, DateTime timePlayed)
        {
            await _service.ScrobbleAsync(_clientService.UserApiClient, GetTrack(track), timePlayed);
            return true;
        }

        public async Task<bool> UpdateNowPlayingAsync(Track track, int duration)
        {
            await _service.UpdateNowPlayingAsync(_clientService.UserApiClient, GetTrack(track), duration);
            return true;
        }

        private LastFmApi.Track GetTrack(Track track)
        {
            return new LastFmApi.Track(track.Title, track.Artist.Name, track.Disc.Album.Title, track.Disc.Album.Artist.Name);
        }
    }
}
