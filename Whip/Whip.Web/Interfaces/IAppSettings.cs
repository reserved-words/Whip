using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Whip.Web.Interfaces
{
    public interface IAppSettings
    {
        int TracksPerPage { get; }
    }
}