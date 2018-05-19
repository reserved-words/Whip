using System.Collections.Generic;
using System.Threading.Tasks;
using Whip.Common.Model;

namespace Whip.Services.Interfaces
{
    public interface IPlayHistoryService
    {
        Task<ICollection<TrackPlay>> GetRecentTrackPlays(int limit);
        Task<ICollection<Statistic>> GetLastWeekTopArtists(int limit);
        Task<ICollection<Statistic>> GetLastYearTopArtists(int limit);
        Task<ICollection<Statistic>> GetOverallTopArtists(int limit);
        Task<ICollection<Statistic>> GetLastWeekTopAlbums(int limit);
        Task<ICollection<Statistic>> GetLastYearTopAlbums(int limit);
        Task<ICollection<Statistic>> GetOverallTopAlbums(int limit);
        Task<ICollection<Statistic>> GetLastWeekTopTracks(int limit);
        Task<ICollection<Statistic>> GetLastYearTopTracks(int limit);
        Task<ICollection<Statistic>> GetOverallTopTracks(int limit);
    }
}
