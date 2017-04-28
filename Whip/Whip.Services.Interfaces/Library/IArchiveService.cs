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
        void ArchiveTrack(Track track);
        void ReinstateTrack(BasicTrackId3Data track);
    }
}
