using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Whip.Common.Model;
using Whip.Common.TagModel;
using Whip.Common.Utilities;
using Whip.Services.Interfaces;
using Whip.Services.Interfaces.Singletons;

namespace Whip.Services
{
    public class ArchiveService : IArchiveService
    {
        private readonly IFileService _fileService;
        private readonly ITaggingService _taggingService;
        private readonly IConfigSettings _configSettings;
        private readonly IUserSettings _userSettings;

        public ArchiveService(IUserSettings userSettings, IFileService fileService, ITaggingService taggingService, IConfigSettings configSettings)
        {
            _fileService = fileService;
            _taggingService = taggingService;
            _userSettings = userSettings;
            _configSettings = configSettings;
        }

        public void ArchiveTrack(Track track)
        {
            throw new NotImplementedException();
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

        public void ReinstateTrack(BasicTrackId3Data track)
        {
            throw new NotImplementedException();
        }
    }
}
