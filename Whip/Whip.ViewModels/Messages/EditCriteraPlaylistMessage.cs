using Whip.Common.Model;

namespace Whip.ViewModels.Messages
{
    public class EditCriteraPlaylistMessage
    {
        public EditCriteraPlaylistMessage(CriteriaPlaylist playlist)
        {
            Playlist = playlist;
        }

        public CriteriaPlaylist Playlist { get; private set; }
    }
}
