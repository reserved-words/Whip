using System.Linq;
using System.Web.Mvc;
using Whip.Common.Singletons;
using Whip.Services.Interfaces;
using Whip.Services.Interfaces.Singletons;
using Whip.Web.Interfaces;

namespace Whip.Web.Controllers
{
    public class LibraryController : BaseController
    {
        public LibraryController(IPlaylist playlist, IErrorLoggingService logger, IPlaylistService playlistsService,
            ICloudService cloudService, ITrackRepository trackRepository)
            : base(trackRepository, cloudService, playlist, logger)
        {
        }

        public JsonResult ShuffleAll()
        {
            return Play("Library", Library.Artists.SelectMany(a => a.Tracks).ToList());
        }
    }
}