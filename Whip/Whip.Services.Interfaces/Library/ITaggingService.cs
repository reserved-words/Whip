using Whip.Common.TagModel;

namespace Whip.Services.Interfaces
{
    public interface ITaggingService
    {
        Id3Data GetTrackId3Data(string filepath);

        void SaveId3Data(string filepath, Id3Data data);
    }
}
