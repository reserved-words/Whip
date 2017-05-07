using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Whip.Common.Model.Rss;

namespace Whip.Services.Interfaces
{
    public interface IRssService
    {
        Task PopulatePosts(List<Feed> feeds);
    }
}
