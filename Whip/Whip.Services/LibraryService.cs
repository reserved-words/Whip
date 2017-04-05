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

        public async Task<Library> GetLibraryAsync(string directory, string[] extensions, IProgress<ProgressArgs> progressHandler)
        {
            return await Task.Run(() =>
            {
                progressHandler?.Report(new ProgressArgs(20, "Processing XML"));

                var library = _dataPersistenceService.GetLibrary();

                var libraryLastUpdated = library.LastUpdated;

                library.LastUpdated = DateTime.Now;

                progressHandler?.Report(new ProgressArgs(40, "Fetching files"));

                var files = _fileService.GetFiles(directory, extensions, libraryLastUpdated);

                progressHandler?.Report(new ProgressArgs(60, "Removing deleted files"));

                _libraryDataOrganiserService.SyncTracks(library.Artists, files.ToKeep);

                progressHandler?.Report(new ProgressArgs(80, "Adding new and modified files"));

                foreach (var file in files.AddedOrModified)
                {
                    _libraryDataOrganiserService.AddTrack(Path.Combine(directory, file.RelativePath), file, library.Artists);
                }

                progressHandler?.Report(new ProgressArgs(100, "Done"));
                
                return library;
            });
        }

        public void SaveLibrary(Library library)
        {
            _dataPersistenceService.Save(library);
        }
    }
}
