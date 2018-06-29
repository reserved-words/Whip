using System.Threading.Tasks;
using System.Web.Mvc;
using System.Web.UI;
using Whip.Services.Interfaces;
using Whip.Services.Interfaces.Singletons;
using Whip.Web.Interfaces;

namespace Whip.Web.Controllers
{
    public class CurrentTrackController : BaseController
    {
        private readonly ITrackLoveService _trackLoveService;

        public CurrentTrackController(ITrackLoveService trackLoveService, ICloudService cloudService, IPlaylist playlist, IErrorLoggingService logger, IPlaySettings playSettings)
            : base(cloudService, playlist, logger, playSettings)
        {
            _trackLoveService = trackLoveService;
        }

        [OutputCache(Duration = 300, VaryByParam = "none", Location = OutputCacheLocation.ServerAndClient)]
        public ActionResult Index()
        {
            var model = GetViewModel(Playlist.CurrentTrack);
            return PartialView("_Index", model);
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