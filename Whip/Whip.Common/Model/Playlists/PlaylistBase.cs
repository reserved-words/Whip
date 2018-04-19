
namespace Whip.Common.Model
{
    public abstract class PlaylistBase
    {
        public PlaylistBase(int id, string title, bool favourite)
        {
            Id = id;
            Title = title;
            Favourite = favourite;
        }

        public int Id { get; set; }
        public string Title { get; set; }
        public bool Favourite { get; set; }
    }
}
