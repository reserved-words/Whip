using System;
using System.Deployment.Application;
using Whip.Services.Interfaces.Utilities;

namespace Whip.Services
{
    public class ApplicationInfoService : IApplicationInfoService
    {
        private const string VersionNumberFormat = "{0}.{1}.{2}.{3}";

        private readonly Version _applicationVersion;
        
        public ApplicationInfoService()
        {
            try
            {
                _applicationVersion = ApplicationDeployment.CurrentDeployment.CurrentVersion;
            }
            catch (InvalidDeploymentException)
            {
                _applicationVersion = null;
            }
        }

        public string Version => GetVersion();
        public DateTime PublishDate => new DateTime(2000, 12, 25);

        private string GetVersion()
        {
            return _applicationVersion == null
                ? "Not installed"
                : string.Format(VersionNumberFormat, 
                    _applicationVersion.Major, 
                    _applicationVersion.Minor, 
                    _applicationVersion.Build, 
                    _applicationVersion.Revision);
        }
    }
}
