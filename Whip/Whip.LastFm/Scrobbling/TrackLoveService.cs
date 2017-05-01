using LastFmApi.Interfaces;
using System.Threading.Tasks;
using Whip.Common.Model;
using Whip.Services.Interfaces;

namespace Whip.LastFm
{
    public class TrackLoveService : Services.Interfaces.ITrackLoveService
    {
        private readonly LastFmApi.Interfaces.ITrackLoveService _service;
        private readonly ILastFmApiClientService _clientService;

        public TrackLoveService(LastFmApi.Interfaces.ITrackLoveService service, ILastFmApiClientService clientService)
        {
            _clientService = clientService;
            _service = service;
        }

        public async Task<bool> IsLovedAsync(Track track)
        {
            return await _service.IsLovedAsync(_clientService.AuthorizedApiClient, GetTrack(track));
        }

        public async Task LoveTrackAsync(Track track)
        {
            await _service.LoveTrackAsync(_clientService.AuthorizedApiClient, GetTrack(track));
        }

        public async Task UnloveTrackAsync(Track track)
        {
            await _service.UnloveTrackAsync(_clientService.AuthorizedApiClient, GetTrack(track));
        }

        private LastFmApi.Track GetTrack(Track track)
        {
            return new LastFmApi.Track(track.Title, track.Artist.Name, track.Disc.Album.Title, track.Disc.Album.Artist.Name);
        }
    }
}
