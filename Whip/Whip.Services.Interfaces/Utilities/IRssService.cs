using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Whip.Common.Model.Rss;

namespace Whip.Services.Interfaces
{
    public interface IRssService
    {
        void PopulatePosts(List<Feed> feeds);
    }
}
