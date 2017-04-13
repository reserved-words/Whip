using LastFmApi.Internal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LastFmApi.Interfaces
{
    public interface ILastFmScrobblingService
    {
        Task ScrobbleAsync(Track track, DateTime timePlayed);
        Task UpdateNowPlayingAsync(Track track, int duration);
    }
}
