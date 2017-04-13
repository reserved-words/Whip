using System;
using System.Threading.Tasks;
using Whip.Common.Model;

namespace Whip.Services.Interfaces
{
    public interface IScrobblingService
    {
        Task ScrobbleAsync(Track track, DateTime timePlayed);
        Task UpdateNowPlayingAsync(Track track, int duration);
    }
}
