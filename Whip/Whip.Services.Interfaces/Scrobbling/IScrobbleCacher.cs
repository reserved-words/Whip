using System;
using System.Collections.Generic;
using Whip.Common.Model;

namespace Whip.Services.Interfaces
{
    public interface IScrobbleCacher
    {
        void Cache(Track track, DateTime timePlayed);
        List<Tuple<Track, DateTime>> GetCachedScrobbles();
        void ReplaceCache(List<Tuple<Track, DateTime, string>> scrobbles);
    }
}
