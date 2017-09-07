using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Whip.Common.Model;

namespace Whip.Services.Interfaces
{
    public interface ITrackQueue
    {
        Track CurrentTrack { get; }
        void Set(List<Track> tracks, int startAt, bool clearCurrentTrack);
        void MoveNext();
        void MovePrevious();
    }
}
