using System.Collections.Generic;
using Whip.Common.Model;
using Whip.Common.Enums;

namespace Whip.ViewModels.Messages
{
    public class PlayArtistsMessage : PlayMessage
    {
        public PlayArtistsMessage(List<Artist> artists, SortType sortType, Track startAt = null)
            : base(sortType, startAt)
        {
            Artists = artists;
        }

        public PlayArtistsMessage(Artist artist, SortType sortType, Track startAt = null)
            : base(sortType, startAt)
        {
            Artists = new List<Artist> { artist };
        }

        public List<Artist> Artists { get; private set; }
    }
}
