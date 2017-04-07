using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Whip.Common.Enums;
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
        private readonly IDirectoryStructureService _directoryStructureService;

        public LibraryService(IFileService fileService, ILibraryDataOrganiserService libraryDataOrganiserService,
            IDataPersistenceService dataPersistenceService, IDirectoryStructureService directoryStructureService)
        {
            _fileService = fileService;
            _libraryDataOrganiserService = libraryDataOrganiserService;
            _dataPersistenceService = dataPersistenceService;
            _directoryStructureService = directoryStructureService;
        }

        public async Task<Library> GetLibraryAsync(string directory, string[] extensions, IProgress<ProgressArgs> progressHandler)
        {
            return await Task.Run(() =>
            {
                progressHandler?.Report(new ProgressArgs(10, "Processing XML"));

                var library = _dataPersistenceService.GetLibrary();

                var libraryLastUpdated = library.LastUpdated;

                library.LastUpdated = DateTime.Now;

                progressHandler?.Report(new ProgressArgs(20, "Fetching files"));

                var files = _fileService.GetFiles(directory, extensions, libraryLastUpdated);

                progressHandler?.Report(new ProgressArgs(40, "Removing deleted files"));

                _libraryDataOrganiserService.SyncTracks(library.Artists, files.ToKeep);

                progressHandler?.Report(new ProgressArgs(60, "Adding new and modified files"));

                foreach (var file in files.AddedOrModified)
                {
                    _libraryDataOrganiserService.AddTrack(file.FullPath, file, library.Artists);
                }

                progressHandler?.Report(new ProgressArgs(80, "Setting artwork paths"));

                foreach (var album in library.Artists.SelectMany(a => a.Albums))
                {
                    album.Artwork = _directoryStructureService.GetArtworkPath(album);
                    album.Grouping = album.ReleaseType.GetReleaseTypeGrouping();
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
