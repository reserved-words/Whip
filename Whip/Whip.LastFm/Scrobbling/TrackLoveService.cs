using LastFmApi.Interfaces;
using System.Threading.Tasks;
using Whip.Common.Model;
using Whip.Services.Interfaces;

namespace Whip.LastFm
{
    public class TrackLoveService : ITrackLoveService
    {
        private readonly ILastFmTrackLoveService _service;

        public TrackLoveService(ILastFmTrackLoveService service)
        {
            _service = service;
        }

        public async Task<bool> IsLovedAsync(Track track)
        {
            return await _service.IsLovedAsync(GetTrack(track));
        }

        public async Task LoveTrackAsync(Track track)
        {
            await _service.LoveTrackAsync(GetTrack(track));
        }

        public async Task UnloveTrackAsync(Track track)
        {
            await _service.UnloveTrackAsync(GetTrack(track));
        }

        private LastFmApi.Track GetTrack(Track track)
        {
            return new LastFmApi.Track(track.Title, track.Artist.Name, track.Disc.Album.Title, track.Disc.Album.Artist.Name);
        }
    }
}
