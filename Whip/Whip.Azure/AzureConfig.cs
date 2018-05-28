using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Whip.Azure
{
    public class AzureConfig : IAzureStorageConfig
    {
        public string AccountName => Properties.Settings.Default.AccountName;
        public string ConnectionString => Properties.Settings.Default.ConnectionString;
        public string ContainerName => Properties.Settings.Default.ContainerName;
    }
}
