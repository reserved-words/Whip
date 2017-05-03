using System.Threading.Tasks;
using Whip.Common.Model;

namespace Whip.Services.Interfaces
{
    public interface ITrackLoveService
    {
        Task<bool> IsLovedAsync(Track track);
        Task<bool> LoveTrackAsync(Track track);
        Task<bool> UnloveTrackAsync(Track track);
    }
}
