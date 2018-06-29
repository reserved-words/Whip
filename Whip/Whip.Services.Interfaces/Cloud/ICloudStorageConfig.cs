using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Whip.Services.Interfaces
{
    public interface ICloudStorageConfig
    {
        string AccountName { get; }
        string ConnectionString { get; }
        string ContainerName { get; }
    }
}
