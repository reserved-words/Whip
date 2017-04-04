using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Whip.Common.Model
{
    public class File
    {
        public File(string relativePath, DateTime dateCreated, DateTime dateModified)
        {
            RelativePath = relativePath;
            DateCreated = dateCreated;
            DateModified = dateModified;
        }

        public string RelativePath { get; private set; }
        public DateTime DateCreated { get; private set; }
        public DateTime DateModified { get; private set; }
    }
}
