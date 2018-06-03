using System.Collections.Generic;
using System.Linq;
using Whip.Common.Model;
using Whip.Common.Singletons;
using Whip.Services.Interfaces;

namespace Whip.Web.Services
{
    public class LibraryService : Interfaces.ILibraryService
    {
        private readonly ITrackRepository _trackRepository;

        private Library _library;

        public LibraryService(ITrackRepository trackRepository)
        {
            _trackRepository = trackRepository;
        }

        public void Reset()
        {
            _library = _trackRepository.GetLibrary();
        }

        public Library Library => _library;

        public List<Track> Tracks => _library.Artists.SelectMany(a => a.Tracks).ToList();
    }
}