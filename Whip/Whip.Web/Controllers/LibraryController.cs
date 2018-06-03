using System.Web.Mvc;
using System.Web.UI;
using Whip.Services.Interfaces;
using Whip.Services.Interfaces.Singletons;

namespace Whip.Web.Controllers
{
    public class LibraryController : BaseController
    {
        private readonly Interfaces.ILibraryService _library;

        public LibraryController(IPlaylist playlist, IErrorLoggingService logger, ICloudService cloudService, Interfaces.ILibraryService library)
            : base(cloudService, playlist, logger)
        {
            _library = library;
        }

        [OutputCache(Duration = 3600, VaryByParam = "none", Location = OutputCacheLocation.Server)]
        public ActionResult Index()
        {
            return PartialView("_Index");
        }

        public JsonResult ShuffleAll()
        {
            return Play("Library", _library.Tracks);
        }
    }
}