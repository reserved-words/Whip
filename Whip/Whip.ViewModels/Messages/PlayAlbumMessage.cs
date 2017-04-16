using System.Collections.Generic;
using Whip.Common;
using Whip.Common.Model;

namespace Whip.ViewModels.Messages
{
    public class PlayAlbumMessage : PlayAllMessage
    {
        public PlayAlbumMessage(Album album, SortType sortType, Track startAt = null)
            : base(sortType, startAt)
        {
            Album = album;
        }

        public Album Album { get; private set; }
    }
}
