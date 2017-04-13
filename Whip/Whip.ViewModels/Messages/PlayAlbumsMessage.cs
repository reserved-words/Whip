using System.Collections.Generic;
using Whip.Common;
using Whip.Common.Model;

namespace Whip.ViewModels.Messages
{
    public class PlayAlbumsMessage : PlayMessage
    {
        public PlayAlbumsMessage(List<Album> albums, SortType sortType, Track startAt = null)
            :base(sortType, startAt)
        {
            Albums = albums;
        }

        public PlayAlbumsMessage(Album album, SortType sortType, Track startAt = null)
            : base(sortType, startAt)
        {
            Albums = new List<Album> { album };
        }

        public List<Album> Albums { get; private set; }
    }
}
