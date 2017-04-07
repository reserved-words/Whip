using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Whip.Common.Model;

namespace Whip.Services.Interfaces
{
    public interface ICommentProcessingService
    {
        string GenerateComment(Track track);

        void Populate(Track track, string comment);

        void Populate(Artist artist, string comment);
    }
}
