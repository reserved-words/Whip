using System.Collections.Generic;
using System.Threading.Tasks;
using Whip.Common.Model;

namespace Whip.Services.Interfaces
{
    public interface IRecentTracksService
    {
        Task<ICollection<TrackPlay>> GetRecentTrackPlays(string username, int limit);
    }
}
