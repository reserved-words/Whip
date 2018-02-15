using System;
using Whip.Common.Model;

namespace Whip.Services.Interfaces
{
    public interface IScrobbleCacher
    {
        void Cache(Track track, DateTime timePlayed);
    }
}
