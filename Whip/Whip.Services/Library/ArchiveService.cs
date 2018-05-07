using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Whip.Common.Model;
using Whip.Common.Singletons;
using Whip.Common.TagModel;
using Whip.Common.Utilities;
using Whip.Services.Interfaces;
using Whip.Services.Interfaces.Singletons;
using static Whip.Common.Resources;

namespace Whip.Services
{
    public class ArchiveService : IArchiveService
    {
        private readonly IFileService _fileService;
        private readonly ITaggingService _taggingService;
        private readonly IConfigSettings _configSettings;
        private readonly IUserSettings _userSettings;
        private readonly ILibraryService _libraryService;
        private readonly Library _library;

        public ArchiveService(IUserSettings userSettings, IFileService fileService, ITaggingService taggingService, 
            IConfigSettings configSettings, ILibraryService libraryService, Library library)
        {
            _fileService = fileService;
            _taggingService = taggingService;
            _userSettings = userSettings;
            _configSettings = configSettings;
            _libraryService = libraryService;
            _library = library;
        }

        public bool ArchiveTracks(List<Track> tracks, out string errorMessage)
        {
            errorMessage = null;

            if (!ArchiveFiles(tracks, out errorMessage))
                return false;

            _libraryService.RemoveTracks(_library, tracks);
            return true;
        }

        public async Task<List<BasicTrackId3Data>> GetArchivedTracksAsync(IProgress<ProgressArgs> progressHandler)
        {
            return await Task.Run(() =>
            {
                if (string.IsNullOrEmpty(_userSettings.ArchiveDirectory))
                {
                    return new List<BasicTrackId3Data>();
                }

                progressHandler?.Report(new ProgressArgs(10, "Getting files"));

                var files = _fileService.GetAllFiles(_userSettings.ArchiveDirectory, _configSettings.FileExtensions);

                progressHandler?.Report(new ProgressArgs(50, "Processing files"));

                var tracks = files
                    .Select(f => _taggingService.GetBasicId3Data(f.FullPath))
                    .OrderBy(t => t.AlbumArtistName)
                    .ThenBy(t => t.Year)
                    .ThenBy(t => t.DiscNo)
                    .ThenBy(t => t.TrackNo)
                    .ToList();

                progressHandler?.Report(new ProgressArgs(100, "Done"));

                return tracks;
            });
        }

        public bool ReinstateTracks(List<BasicTrackId3Data> tracks, out string errorMessage)
        {
            throw new NotImplementedException();
        }

        private bool ArchiveFiles(List<Track> tracks, out string errorMessage)
        {
            errorMessage = null;

            var archiveDirectory = _userSettings.ArchiveDirectory;

            if (string.IsNullOrEmpty(archiveDirectory))
            {
                errorMessage = ErrorNoArchiveDirectorySet;
                return false;
            }

            foreach (var track in tracks)
            {
                var artistDirectory = string.IsNullOrEmpty(track.Disc.Album.Artist.Name)
                    ? "Unknown"
                    : track.Disc.Album.Artist.Name;

                var albumDirectory = string.IsNullOrEmpty(track.Disc.Album.Title)
                    ? "Unknown"
                    : track.Disc.Album.Title;

                var directory = _fileService.CreateDirectory(archiveDirectory, artistDirectory, albumDirectory);

                _fileService.CopyFile(track.File.FullPath, directory);
            }

            return true;
        }
    }
}
