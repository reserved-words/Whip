
using Whip.Common.Model;

namespace Whip.Services.Interfaces
{
    public interface ITrackUpdateService
    {
        void SaveTrackChanges(Track trackChanged, Artist originalArtist, Disc originalDisc);
    }
}
