using Whip.Common.Model;

namespace Whip.ViewModels.Messages
{
    public class EditCriteriaPlaylistMessage
    {
        public EditCriteriaPlaylistMessage(CriteriaPlaylist playlist)
        {
            Playlist = playlist;
        }

        public CriteriaPlaylist Playlist { get; private set; }
    }
}
