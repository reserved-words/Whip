using Whip.AzureSync;
using Whip.NLog;
using Whip.Services;
using Whip.XmlDataAccess;

namespace Whip.CloudSync
{
    class Program
    {
        static void Main(string[] args)
        {
            var cloudService = new AzureService();

            var xmlFileService = new XmlFileService();
            var trackXmlParser = new TrackXmlParser();
            var trackRepository = new TrackRepository(null, xmlFileService, trackXmlParser);

            var logger = new LoggingService();

            var service = new Service(trackRepository, cloudService, logger);
            service.Run();
        }
    }
}
