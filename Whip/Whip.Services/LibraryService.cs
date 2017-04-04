using System;
using System.Collections.Generic;
using System.IO;
using Whip.Common.Model;
using Whip.Services.Interfaces;

namespace Whip.Services
{
    public class LibraryService : ILibraryService
    {
        private readonly IFileService _fileService;
        private readonly ILibraryDataOrganiserService _libraryDataOrganiserService;
        private readonly IDataPersistenceService _dataPersistenceService;

        public LibraryService(IFileService fileService, ILibraryDataOrganiserService libraryDataOrganiserService,
            IDataPersistenceService dataPersistenceService)
        {
            _fileService = fileService;
            _libraryDataOrganiserService = libraryDataOrganiserService;
            _dataPersistenceService = dataPersistenceService;
        }

        public ICollection<Artist> GetLibrary(string directory, params string[] extensions)
        {
            var files = _fileService.GetFiles(directory, extensions);

            var artists = new List<Artist>();

            foreach (var file in files)
            {
                _libraryDataOrganiserService.AddTrack(Path.Combine(directory, file.RelativePath), file, artists);
            }

            return artists;
        }

        public void SaveLibrary(ICollection<Artist> artists)
        {
            _dataPersistenceService.Save(artists);
        }
    }
}
