﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Whip.Common.Model;

namespace Whip.ViewModels.Singletons.Interfaces
{
    public interface ILibrary
    {
        ICollection<Artist> Artists { get; set; }
    }
}
