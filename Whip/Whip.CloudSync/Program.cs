using System;
using Whip.AzureSync;
using Whip.Services;
using Whip.Services.Singletons;
using Whip.TagLibSharp;
using Whip.XmlDataAccess;

namespace Whip.CloudSync
{
    class Program
    {
        static void Main(string[] args)
        {
            var webHelper = new WebHelperService();
            var configSettings = new ConfigSettings();
            var logger = new ErrorLoggingService(webHelper, configSettings);

            try
            {
                var xmlFileService = new XmlFileService();
                var trackXmlParser = new TrackXmlParser();
                var syncData = new SyncData();

                var trackRepository = new TrackRepository(syncData, xmlFileService, trackXmlParser);
                var cloudService = new AzureService(syncData);
                var taggingService = new TagLibService();
                
                var service = new Service(trackRepository, cloudService, logger, syncData, taggingService);

                service.Run();
            }
            catch (Exception ex)
            {
                logger.Log(ex);
            }
        }
    }
}
