using System.Collections.Generic;
using Whip.Common.Model.Rss;

namespace Whip.Services.Interfaces
{
    public interface IRssFeedsRepository
    {
        List<Feed> GetFeeds();

        void SaveFeeds(List<Feed> feeds);
    }
}
