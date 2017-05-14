
namespace Whip.Common.Model
{
    public abstract class PlaylistBase
    {
        public PlaylistBase(int id, string title)
        {
            Id = id;
            Title = title;
        }

        public int Id { get; set; }
        public string Title { get; set; }
    }
}
