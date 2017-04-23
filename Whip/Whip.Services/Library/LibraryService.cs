using System;
using System.Linq;
using System.Threading.Tasks;
using Whip.Common;
using Whip.Common.Singletons;
using Whip.Common.Utilities;
using Whip.Services.Interfaces;

namespace Whip.Services
{
    public class LibraryService : ILibraryService
    {
        private readonly IFileService _fileService;
        private readonly ILibraryDataOrganiserService _libraryDataOrganiserService;
        private readonly ILibrarySortingService _librarySortingService;
        private readonly IDataPersistenceService _dataPersistenceService;
        private readonly IUserSettings _userSettings;

        public LibraryService(IFileService fileService, ILibraryDataOrganiserService libraryDataOrganiserService,
            IDataPersistenceService dataPersistenceService, IUserSettings userSettings, ILibrarySortingService librarySortingService)
        {
            _fileService = fileService;
            _libraryDataOrganiserService = libraryDataOrganiserService;
            _librarySortingService = librarySortingService;
            _dataPersistenceService = dataPersistenceService;
            _userSettings = userSettings;
        }

        public async Task<Library> GetLibraryAsync(IProgress<ProgressArgs> progressHandler)
        {
            return await Task.Run(() =>
            {
                progressHandler?.Report(new ProgressArgs(10, "Processing XML"));

                var library = _dataPersistenceService.GetLibrary();

                var libraryLastUpdated = library.LastUpdated;

                var newUpdateDate = DateTime.Now;

                progressHandler?.Report(new ProgressArgs(20, "Fetching files"));

                var files = _fileService.GetFiles(_userSettings.MusicDirectory, ApplicationSettings.FileExtensions, libraryLastUpdated);

                progressHandler?.Report(new ProgressArgs(40, "Removing deleted files"));

                _libraryDataOrganiserService.SyncTracks(library.Artists, files.ToKeep);

                progressHandler?.Report(new ProgressArgs(60, "Adding new and modified files"));

                foreach (var file in files.AddedOrModified)
                {
                    _libraryDataOrganiserService.AddTrack(file.FullPath, file, library.Artists);
                }

                progressHandler?.Report(new ProgressArgs(80, "Setting release type groupings"));

                foreach (var album in library.Artists.SelectMany(a => a.Albums))
                {
                    album.Grouping = album.ReleaseType.GetReleaseTypeGrouping();
                }

                progressHandler?.Report(new ProgressArgs(90, "Sorting artists"));

                library.Artists = _librarySortingService.GetInDefaultOrder(library.Artists).ToList();

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
