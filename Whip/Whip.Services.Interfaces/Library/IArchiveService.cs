using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Whip.Common.Model;
using Whip.Common.TagModel;
using Whip.Common.Utilities;

namespace Whip.Services.Interfaces
{
    public interface IArchiveService
    {
        Task<List<BasicTrackId3Data>> GetArchivedTracksAsync(IProgress<ProgressArgs> progressHandler);
        bool ArchiveTracks(List<Track> tracks, out string errorMessage);
        bool ReinstateTracks(List<BasicTrackId3Data> tracks, out string errorMessage);
    }
}
