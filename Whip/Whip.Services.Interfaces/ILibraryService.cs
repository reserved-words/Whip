﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Whip.Common.Model;

namespace Whip.Services.Interfaces
{
    public interface ILibraryService
    {
        ICollection<Artist> GetLibrary(string directory, params string[] extensions);
        void SaveLibrary(ICollection<Artist> artists);
    }
}
