
using Whip.Common.Enums;

namespace Whip.Common.Model
{
    public class Playlist
    {
        public Playlist(PlaylistType type, int id, string title, bool favourite)
        {
            Type = type;
            Id = id;
            Title = title;
            Favourite = favourite;
        }

        public PlaylistType Type { get; set; }
        public int Id { get; set; }
        public string Title { get; set; }
        public bool Favourite { get; set; }
    }
}
