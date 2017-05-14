using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Whip.Common.Model;

namespace Whip.Services.Interfaces
{
    public interface IPlaylistRepository
    {
        AllPlaylists GetPlaylists();

        void Save(OrderedPlaylist playlist);

        void Save(CriteriaPlaylist playlist);

        void Delete(CriteriaPlaylist playlist);
    }
}
