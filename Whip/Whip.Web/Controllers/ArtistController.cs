using System.Web.Mvc;
using Whip.Services.Interfaces;
using Whip.Services.Interfaces.Singletons;
using Whip.Web.Interfaces;

namespace Whip.Web.Controllers
{
    public class ArtistController : BaseController
    {
        public ArtistController(IPlaylist playlist, IErrorLoggingService logger, ICloudService cloudService, IPlaySettings playSettings)
            : base(cloudService, playlist, logger, playSettings)
        {
        }

        public ActionResult Index()
        {
            return PartialView("_Index");
        }
    }
}