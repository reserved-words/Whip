using Whip.Common.Model;

namespace Whip.ViewModels.Messages
{
    public class MoveToTrackMessage
    {
        public MoveToTrackMessage(Track track)
        {
            Track = track;
        }

        public Track Track { get; private set; }
    }
}
