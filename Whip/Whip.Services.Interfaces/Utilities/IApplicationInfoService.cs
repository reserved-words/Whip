using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Whip.Services.Interfaces.Utilities
{
    public interface IApplicationInfoService
    {
        string Version { get; }
        DateTime PublishDate { get; }
    }
}
