using System;
using System.Linq;
using System.Threading.Tasks;
using Whip.Common;
using Whip.Common.Singletons;
using Whip.Common.Utilities;
using Whip.Services.Interfaces;
using Whip.Services.Interfaces.Singletons;

namespace Whip.Services
{
    public class LibraryService : ILibraryService
    {
        private readonly IFileService _fileService;
        private readonly ILibraryDataOrganiserService _libraryDataOrganiserService;
        private readonly ILibrarySortingService _librarySortingService;
        private readonly ITrackRepository _dataPersistenceService;
        private readonly ITaggingService _taggingService;
        private readonly IUserSettings _userSettings;
        private readonly IConfigSettings _configSettings;

        public LibraryService(IFileService fileService, ILibraryDataOrganiserService libraryDataOrganiserService, ITaggingService taggingService,
            ITrackRepository dataPersistenceService, IUserSettings userSettings, ILibrarySortingService librarySortingService,
            IConfigSettings configSettings)
        {
            _fileService = fileService;
            _libraryDataOrganiserService = libraryDataOrganiserService;
            _librarySortingService = librarySortingService;
            _dataPersistenceService = dataPersistenceService;
            _taggingService = taggingService;
            _userSettings = userSettings;
            _configSettings = configSettings;
        }

        public async Task<Library> GetLibraryAsync(IProgress<ProgressArgs> progressHandler)
        {
            return await Task.Run(() =>
            {
                progressHandler?.Report(new ProgressArgs(10, "Processing XML"));

                var library = _dataPersistenceService.GetLibrary();

                var libraryLastUpdated = library.LastUpdated;

                library.LastUpdated = DateTime.Now;

                progressHandler?.Report(new ProgressArgs(10, "Fetching files"));

                var files = _fileService.GetFiles(_userSettings.MusicDirectory, _configSettings.FileExtensions, libraryLastUpdated);

                progressHandler?.Report(new ProgressArgs(20, "Removing deleted files"));

                _libraryDataOrganiserService.SyncTracks(library.Artists, files.ToKeep);

                var total = files.AddedOrModified.Count;
                var count = 0;
                var range = 80 - 30;

                foreach (var file in files.AddedOrModified)
                {
                    progressHandler?.Report(new ProgressArgs(30 + (range * count / total), "Adding new and modified files"));

                    _libraryDataOrganiserService.AddTrack(file.FullPath, file, library.Artists);

                    count++;
                }

                var albums = library.Artists.SelectMany(a => a.Albums).ToList();

                total = albums.Count;
                count = 0;
                range = 90 - 80;

                foreach (var album in albums)
                {
                    progressHandler?.Report(new ProgressArgs(80 + (range * count / total), "Setting artwork and release type groupings"));

                    if (album.Artwork == null)
                    {
                        album.Artwork = _taggingService.GetArtworkBytes(album.Discs.First().Tracks.First().File.FullPath);
                    }
                    album.Grouping = album.ReleaseType.GetReleaseTypeGrouping();

                    count++;
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
