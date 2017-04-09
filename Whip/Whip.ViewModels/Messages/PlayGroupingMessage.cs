using Whip.Common.Model;
using Whip.Common.Enums;

namespace Whip.ViewModels.Messages
{
    public class PlayGroupingMessage : PlayMessage
    {
        public PlayGroupingMessage(string grouping, SortType sortType, Track startAt = null)
            : base(sortType, startAt)
        {
            Grouping = grouping;
        }

        public string Grouping { get; private set; }
    }
}
