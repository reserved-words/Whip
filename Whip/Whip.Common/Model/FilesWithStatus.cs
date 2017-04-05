using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Whip.Common.Model
{
    public class FilesWithStatus
    {
        public FilesWithStatus()
        {
            AddedOrModified = new List<File>();
            ToKeep = new List<string>();
        }

        public ICollection<File> AddedOrModified { get; set; }
        public ICollection<string> ToKeep { get; set; }
    }
}
