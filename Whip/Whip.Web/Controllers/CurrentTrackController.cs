using System.Threading.Tasks;
using System.Web.Mvc;
using Whip.Services.Interfaces;
using Whip.Services.Interfaces.Singletons;

namespace Whip.Web.Controllers
{
    public class CurrentTrackController : BaseController
    {
        private readonly ITrackLoveService _trackLoveService;

        public CurrentTrackController(ITrackLoveService trackLoveService, ICloudService cloudService, IPlaylist playlist, IErrorLoggingService logger)
            : base(cloudService, playlist, logger)
        {
            _trackLoveService = trackLoveService;
        }

        public ActionResult Index()
        {
            return PartialView("_Index");
        }

        [HttpPost]
        public async Task<JsonResult> IsLoved()
        {
            var track = Playlist.CurrentTrack;

            return new JsonResult
            {
                Data = track == null
                    ? null
                    : new
                    {
                        IsLoved = await _trackLoveService.IsLovedAsync(track)
                    }
            };
        }
    }
}