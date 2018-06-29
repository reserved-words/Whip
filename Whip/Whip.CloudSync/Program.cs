using System;
using Whip.Azure;
using Whip.Services;
using Whip.TagLibSharp;
using Whip.XmlDataAccess;

namespace Whip.CloudSync
{
    class Program
    {
        static void Main(string[] args)
        {
            var syncData = new SyncData();
            var configXmlProvider = new ConfigXmlProvider(syncData);
            var webHelper = new WebHelperService();
            var configSettings = new ConfigSettings(configXmlProvider);
            var logger = new ErrorLoggingService(webHelper, configSettings);

            try
            {
                var trackXmlParser = new TrackXmlParser();
                var trackXmlProvider = new TrackXmlProvider(syncData);
                var trackRepository = new TrackRepository(trackXmlParser, trackXmlProvider);
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
