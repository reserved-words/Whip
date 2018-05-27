using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Whip.Azure
{
    public interface IAzureStorageConfig
    {
        string AccountName { get; }
        string ConnectionString { get; }
        string ContainerName { get; }
    }
}
