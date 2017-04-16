using System.Collections.Generic;
using Whip.Common.Model;
using Whip.Common;

namespace Whip.ViewModels.Messages
{
    public class PlayArtistMessage : PlayAllMessage
    {
        public PlayArtistMessage(Artist artist, SortType sortType, Track startAt = null)
            : base(sortType, startAt)
        {
            Artist = artist;
        }

        public Artist Artist { get; private set; }
    }
}
