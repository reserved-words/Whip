using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Whip.Common.Model;
using Whip.Common.Utilities;
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

        public async Task<ICollection<Artist>> GetLibraryAsync(string directory, string[] extensions, IProgress<ProgressArgs> progressHandler)
        {
            return await Task.Run(() =>
            {
                progressHandler?.Report(new ProgressArgs(25, "Processing XML"));

                var library = _dataPersistenceService.GetLibrary();

                progressHandler?.Report(new ProgressArgs(50, "Fetching files"));

                var files = _fileService.GetFiles(directory, extensions);

                progressHandler?.Report(new ProgressArgs(75, "Processing files"));

                var artists = new List<Artist>();

                foreach (var file in files)
                {
                    _libraryDataOrganiserService.AddTrack(Path.Combine(directory, file.RelativePath), file, artists);
                }

                progressHandler?.Report(new ProgressArgs(100, "Done"));

                return artists;
            });
        }

        public void SaveLibrary(ICollection<Artist> artists)
        {
            _dataPersistenceService.Save(artists);
        }
    }
}
