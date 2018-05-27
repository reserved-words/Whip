using System;
using System.Collections.Generic;
using System.Linq;
using Whip.Common.Model;
using Whip.Services.Interfaces;

namespace Whip.CloudSync
{
    public class Service
    {
        private readonly ILoggingService _logger;
        private readonly ITrackRepository _repository;
        private readonly ICloudService _cloudService;

        public Service(ITrackRepository repository, ICloudService cloudService, ILoggingService logger)
        {
            _cloudService = cloudService;
            _logger = logger;
            _repository = repository;
        }

        public void Run()
        {
            var tracksToUpload = GetTracksToUpload(DateTime.MinValue);
            foreach (var track in tracksToUpload)
            {
                _cloudService.Upload(track);
            }
        }

        private List<Track> GetTracksToUpload(DateTime timeLastUpdated)
        {
            var library = _repository.GetLibrary();

            return library.Artists
                .SelectMany(a => a.Albums)
                .SelectMany(a => a.Discs)
                .SelectMany(d => d.Tracks)
                .Where(t => t.File.DateModified >= timeLastUpdated)
                .ToList();
        }
    }
}
