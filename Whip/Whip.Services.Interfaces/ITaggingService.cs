using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Whip.Common.Model;

namespace Whip.Services.Interfaces
{
    public interface ITaggingService
    {
        TrackId3Data GetId3Data(string filepath);
    }
}
