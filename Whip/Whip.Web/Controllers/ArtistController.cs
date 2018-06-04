using System.Web.Mvc;
using Whip.Services.Interfaces;
using Whip.Services.Interfaces.Singletons;

namespace Whip.Web.Controllers
{
    public class ArtistController : BaseController
    {
        public ArtistController(IPlaylist playlist, IErrorLoggingService logger, ICloudService cloudService)
            : base(cloudService, playlist, logger)
        {
        }

        public ActionResult Index()
        {
            return PartialView("_Index");
        }
    }
}