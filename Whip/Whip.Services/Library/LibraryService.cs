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
        private readonly IDataPersistenceService _dataPersistenceService;
        private readonly IDirectoryStructureService _directoryStructureService;
        private readonly IUserSettingsService _userSettingsService;

        public LibraryService(IFileService fileService, ILibraryDataOrganiserService libraryDataOrganiserService,
            IDataPersistenceService dataPersistenceService, IDirectoryStructureService directoryStructureService,
            IUserSettingsService userSettingsService)
        {
            _fileService = fileService;
            _libraryDataOrganiserService = libraryDataOrganiserService;
            _dataPersistenceService = dataPersistenceService;
            _directoryStructureService = directoryStructureService;
            _userSettingsService = userSettingsService;
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

                var files = _fileService.GetFiles(_userSettingsService.MusicDirectory, ApplicationSettings.FileExtensions, libraryLastUpdated);

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

                progressHandler?.Report(new ProgressArgs(90, "Sorting artists"));

                library.Artists = library.Artists.OrderBy(a => a.Name).ToList();

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
