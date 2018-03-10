using LastFmApi.Interfaces;
using System.Collections.Generic;
using System.Threading.Tasks;
using Whip.Common.Model;
using Whip.Services.Interfaces;
using System.Linq;
using LastFmApi.Model;

namespace Whip.LastFm
{
    public class UserInfoService : IRecentTracksService
    {
        private readonly ILastFmApiClientService _clientService;
        private readonly IUserInfoService _userInfoService;
        private readonly ITimeToString _timeToString;

        public UserInfoService(ILastFmApiClientService clientService, IUserInfoService userInfoService,
            ITimeToString timeToString)
        {
            _userInfoService = userInfoService;
            _clientService = clientService;
            _timeToString = timeToString;
        }

        public async Task<ICollection<TrackPlay>> GetRecentTrackPlays(string username, int limit)
        {
            var tracks = await _userInfoService.GetRecentTracks(_clientService.ApiClient, username, limit);

            return tracks.Select(rt => new TrackPlay
                {
                    Track = $"{rt.ArtistName} - {rt.TrackTitle}",
                    TimePlayed = GetTimePlayed(rt),
                    NowPlaying = rt.NowPlaying,
                    Url = rt.Url,
                    ImageUrl = rt.ImageUrl
                })
                .ToList();
        }

        private string GetTimePlayed(TrackPlayInfo trackPlay)
        {
            return trackPlay.NowPlaying
                ? "Playing now"
                : trackPlay.TimePlayed.HasValue
                    ? _timeToString.GetTimeAsTimeAgo(trackPlay.TimePlayed.Value, 5 * 60, false)
                    : "";
        }
    }
}
