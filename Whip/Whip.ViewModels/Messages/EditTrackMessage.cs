using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Whip.Common.Model;

namespace Whip.ViewModels.Messages
{
    public class EditTrackMessage
    {
        public EditTrackMessage(Track track)
        {
            Track = track;
        }

        public Track Track { get; private set; }
    }
}
