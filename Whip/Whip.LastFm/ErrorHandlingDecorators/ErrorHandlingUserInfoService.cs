using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Whip.Common.Enums;
using Whip.Common.Model;
using Whip.Services.Interfaces;

namespace Whip.LastFm.ErrorHandlingDecorators
{
    public class ErrorHandlingUserInfoService : IPlayHistoryService
    {
        private readonly IPlayHistoryService _service;
        private readonly IAsyncMethodInterceptor _interceptor;

        public ErrorHandlingUserInfoService(IPlayHistoryService service, IAsyncMethodInterceptor interceptor)
        {
            _interceptor = interceptor;
            _service = service;
        }

        public async Task<ICollection<TrackPlay>> GetRecentTrackPlays(int limit)
        {
            return await _interceptor.TryMethod(
                _service.GetRecentTrackPlays(limit),
                new List<TrackPlay>(),
                WebServiceType.LastFm,
                "Get Recent Tracks (Limit: " + limit + ")");
        }

        public async Task<ICollection<Statistic>> GetLastWeekTopArtists(int limit)
        {
            return await GetStatistics(i => _service.GetLastWeekTopArtists(i), limit, "Get Top Artists Week (Limit: " + limit + ")");
        }

        public async Task<ICollection<Statistic>> GetLastYearTopArtists(int limit)
        {
            return await GetStatistics(i => _service.GetLastYearTopArtists(i), limit, "Get Top Artists Year (Limit: " + limit + ")");
        }

        public async Task<ICollection<Statistic>> GetOverallTopArtists(int limit)
        {
            return await GetStatistics(i => _service.GetOverallTopArtists(i), limit, "Get Top Artists Overall (Limit: " + limit + ")");
        }

        public async Task<ICollection<Statistic>> GetLastWeekTopAlbums(int limit)
        {
            return await GetStatistics(i => _service.GetLastWeekTopAlbums(i), limit, "Get Top Albums Week (Limit: " + limit + ")");
        }

        public async Task<ICollection<Statistic>> GetLastYearTopAlbums(int limit)
        {
            return await GetStatistics(i => _service.GetLastYearTopAlbums(i), limit, "Get Top Albums Year (Limit: " + limit + ")");
        }

        public async Task<ICollection<Statistic>> GetOverallTopAlbums(int limit)
        {
            return await GetStatistics(i => _service.GetOverallTopAlbums(i), limit, "Get Top Albums Overall (Limit: " + limit + ")");
        }

        public async Task<ICollection<Statistic>> GetLastWeekTopTracks(int limit)
        {
            return await GetStatistics(i => _service.GetLastWeekTopTracks(i), limit, "Get Top Tracks Week (Limit: " + limit + ")");
        }

        public async Task<ICollection<Statistic>> GetLastYearTopTracks(int limit)
        {
            return await GetStatistics(i => _service.GetLastYearTopTracks(i), limit, "Get Top Tracks Year (Limit: " + limit + ")");
        }

        public async Task<ICollection<Statistic>> GetOverallTopTracks(int limit)
        {
            return await GetStatistics(i => _service.GetOverallTopTracks(i), limit, "Get Top Tracks Overall (Limit: " + limit + ")");
        }

        private async Task<ICollection<Statistic>> GetStatistics(Func<int, Task<ICollection<Statistic>>> function, int limit, string description)
        {
            return await _interceptor.TryMethod(
                function(limit),
                new List<Statistic>(),
                WebServiceType.LastFm,
                description);
        }
    }
}
