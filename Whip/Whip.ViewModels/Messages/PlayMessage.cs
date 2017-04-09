using GalaSoft.MvvmLight.Messaging;
using Whip.Common.Model;
using Whip.Common.Enums;

namespace Whip.ViewModels.Messages
{
    public class PlayMessage : MessageBase
    {
        public PlayMessage(SortType sortType, Track startAt = null)
        {
            SortType = sortType;
            StartAt = startAt;
        }

        public SortType SortType { get; private set; }
        public Track StartAt { get; private set; }
    }
}
