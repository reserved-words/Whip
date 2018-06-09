using System.Linq;
using System.Web.Mvc;
using System.Web.UI;
using Whip.Services.Interfaces;
using Whip.Services.Interfaces.Singletons;
using Whip.Web.Models;

namespace Whip.Web.Controllers
{
    public class LibraryController : BaseController
    {
        private readonly ICloudService _cloudService;
        private readonly Interfaces.ILibraryService _library;

        public LibraryController(IPlaylist playlist, IErrorLoggingService logger, ICloudService cloudService, Interfaces.ILibraryService library)
            : base(cloudService, playlist, logger)
        {
            _cloudService = cloudService;
            _library = library;
        }

        [OutputCache(Duration = 3600, VaryByParam = "none", Location = OutputCacheLocation.Server)]
        public ActionResult Index()
        {
            var model = new LibraryViewModel
            {
                Artists = _library.Library.Artists
                    .Where(a => a.Albums.Any())
                    .Select(a => new ArtistViewModel
                    {
                        Name = a.Name,
                        PlayUrl = Url.Action("PlayArtist", new { name = a.Name })
                    })
                    .ToList()
            };

            return PartialView("_Index", model);
        }

        public ActionResult Artist(string name)
        {
            var artist = _library.Library.Artists.Single(a => a.Name == name);
            var model = new LibraryArtistViewModel
            {
                Name = artist.Name,
                Albums = artist.Albums
                    .Select(a => new LibraryAlbumViewModel
                    {
                        Title = a.Title,
                        ArtworkUrl = _cloudService.GetArtworkUrl(a),
                        Tracks = a.Discs.SelectMany(d => d.Tracks).Select(t => t.Title).ToList()
                    })
                    .ToList()
            };

            return PartialView("_Artist", model);
        }

        public JsonResult ShuffleAll()
        {
            return Play("Library", _library.Tracks);
        }

        public JsonResult PlayArtist(string name)
        {
            var artist = _library.Library.Artists.Single(a => a.Name == name);
            return Play(artist.Name, artist.Albums.SelectMany(a => a.Discs.SelectMany(d => d.Tracks)).ToList());
        }
    }
}