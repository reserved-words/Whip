using Whip.Common.Model;
using Whip.Common;

namespace Whip.ViewModels.Messages
{
    public class PlayAllMessage : PlayMessage
    {
        public PlayAllMessage(SortType? sortType, Track startAt = null)
            : base(sortType, startAt)
        {
        }
    }
}
