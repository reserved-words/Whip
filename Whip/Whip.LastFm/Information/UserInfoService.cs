using LastFmApi.Interfaces;
using System.Collections.Generic;
using System.Threading.Tasks;
using Whip.Common.Model;
using Whip.Services.Interfaces;
using System.Linq;
using LastFmApi.Model;

namespace Whip.LastFm
{
    public class UserInfoService : IPlayHistoryService
    {
        private readonly ILastFmApiClientService _clientService;
        private readonly IUserInfoService _userInfoService;
        private readonly ITimeToString _timeToString;
        private readonly IUserSettings _userSettings;

        public UserInfoService(ILastFmApiClientService clientService, IUserInfoService userInfoService,
            ITimeToString timeToString, IUserSettings userSettings)
        {
            _userInfoService = userInfoService;
            _clientService = clientService;
            _timeToString = timeToString;
            _userSettings = userSettings;
        }

        public async Task<ICollection<TrackPlay>> GetRecentTrackPlays(int limit)
        {
            if (string.IsNullOrEmpty(_userSettings.LastFmUsername))
                return new List<TrackPlay>();

            var tracks = await _userInfoService.GetRecentTracks(_clientService.ApiClient, _userSettings.LastFmUsername, limit);

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

        public async Task<ICollection<Statistic>> GetLastWeekTopArtists(int limit)
        {
            return await GetTopArtists(limit, TimePeriod.Week);
        }

        public async Task<ICollection<Statistic>> GetLastYearTopArtists(int limit)
        {
            return await GetTopArtists(limit, TimePeriod.Year);
        }

        public async Task<ICollection<Statistic>> GetOverallTopArtists(int limit)
        {
            return await GetTopArtists(limit, TimePeriod.Overall);
        }

        public async Task<ICollection<Statistic>> GetLastWeekTopAlbums(int limit)
        {
            return await GetTopAlbums(limit, TimePeriod.Week);
        }

        public async Task<ICollection<Statistic>> GetLastYearTopAlbums(int limit)
        {
            return await GetTopAlbums(limit, TimePeriod.Year);
        }

        public async Task<ICollection<Statistic>> GetOverallTopAlbums(int limit)
        {
            return await GetTopAlbums(limit, TimePeriod.Overall);
        }

        public async Task<ICollection<Statistic>> GetLastWeekTopTracks(int limit)
        {
            return await GetTopTracks(limit, TimePeriod.Week);
        }

        public async Task<ICollection<Statistic>> GetLastYearTopTracks(int limit)
        {
            return await GetTopTracks(limit, TimePeriod.Year);
        }

        public async Task<ICollection<Statistic>> GetOverallTopTracks(int limit)
        {
            return await GetTopTracks(limit, TimePeriod.Overall);
        }

        private string GetTimePlayed(TrackPlayInfo trackPlay)
        {
            return trackPlay.NowPlaying
                ? "Playing now"
                : trackPlay.TimePlayed.HasValue
                    ? _timeToString.GetTimeAsTimeAgo(trackPlay.TimePlayed.Value, 5 * 60, false)
                    : "";
        }

        private async Task<ICollection<Statistic>> GetTopArtists(int limit, TimePeriod period)
        {
            if (string.IsNullOrEmpty(_userSettings.LastFmUsername))
                return new List<Statistic>();

            var artists = await _userInfoService.GetTopArtists(_clientService.ApiClient, _userSettings.LastFmUsername, period, limit);

            return artists.Select(a => new Statistic(a.Name, a.PlayCount, a.Url, a.ImageUrl)).ToList();
        }

        private async Task<ICollection<Statistic>> GetTopAlbums(int limit, TimePeriod period)
        {
            if (string.IsNullOrEmpty(_userSettings.LastFmUsername))
                return new List<Statistic>();

            var albums = await _userInfoService.GetTopAlbums(_clientService.ApiClient, _userSettings.LastFmUsername, period, limit);

            return albums.Select(a => new Statistic($"{a.ArtistName} - {a.Title}", a.PlayCount, a.Url, a.ImageUrl)).ToList();
        }

        private async Task<ICollection<Statistic>> GetTopTracks(int limit, TimePeriod period)
        {
            if (string.IsNullOrEmpty(_userSettings.LastFmUsername))
                return new List<Statistic>();

            var tracks = await _userInfoService.GetTopTracks(_clientService.ApiClient, _userSettings.LastFmUsername, period, limit);

            return tracks.Select(a => new Statistic($"{a.ArtistName} - {a.Title}", a.PlayCount, a.Url, a.ImageUrl)).ToList();
        }
    }
}
