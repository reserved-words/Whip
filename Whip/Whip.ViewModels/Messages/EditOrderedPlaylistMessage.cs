using Whip.Common.Model;

namespace Whip.ViewModels.Messages
{
    public class EditOrderedPlaylistMessage
    {
        public EditOrderedPlaylistMessage(OrderedPlaylist playlist)
        {
            Playlist = playlist;
        }

        public OrderedPlaylist Playlist { get; private set; }
    }
}
