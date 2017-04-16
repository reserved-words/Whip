using Whip.Common;
using Whip.Common.Model;

namespace Whip.ViewModels.Messages
{
    public class PlayMessage
    {
        public PlayMessage(SortType? sortType, Track startAt = null)
        {
            SortType = sortType;
            StartAt = startAt;
        }

        public SortType? SortType { get; private set; }
        public Track StartAt { get; private set; }
    }
}
