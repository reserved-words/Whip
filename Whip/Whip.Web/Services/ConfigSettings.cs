using System;
using System.Collections.Generic;
using System.Configuration;
using Whip.Services.Interfaces;
using Whip.Services.Interfaces.Singletons;

namespace Whip.Web.Services
{
    public class ConfigSettings : IConfigSettings, ICloudStorageConfig
    {
        public string AccountName => ConfigurationManager.AppSettings["CloudAccountName"];
        public string ConnectionString => ConfigurationManager.AppSettings["CloudConnectionString"];
        public string ContainerName => ConfigurationManager.AppSettings["CloudContainerName"];
        public string BandsInTownApiKey => ConfigurationManager.AppSettings["BandsInTownApiKey"];
        public string TwitterApiAccessToken => ConfigurationManager.AppSettings["TwitterApiAccessToken"];
        public string TwitterApiAccessTokenSecret => ConfigurationManager.AppSettings["TwitterApiAccessTokenSecret"];
        public string TwitterApiKey => ConfigurationManager.AppSettings["TwitterApiKey"];
        public string TwitterApiSecret => ConfigurationManager.AppSettings["TwitterApiSecret"];
        public string YouTubeApiKey => ConfigurationManager.AppSettings["YouTubeApiKey"];
        public string LastFmApiKey => ConfigurationManager.AppSettings["LastFmApiKey"];
        public string LastFmApiSecret => ConfigurationManager.AppSettings["LastFmApiSecret"];
        public string LoggingUrl => ConfigurationManager.AppSettings["LoggingUrl"];
        public int MinutesBeforeRefreshNews => int.Parse(ConfigurationManager.AppSettings["MinutesBeforeRefreshNews"]);
        public int MinutesBeforeUpdatingTweets => int.Parse(ConfigurationManager.AppSettings["MinutesBeforeUpdatingTweets"]);
        public int NumberOfSimilarArtistsToDisplay => int.Parse(ConfigurationManager.AppSettings["NumberOfSimilarArtistsToDisplay"]);

        public int DaysBeforeUpdatingArtistWebInfo
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        public List<string> FileExtensions
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        public int TrackChangeDelay
        {
            get
            {
                throw new NotImplementedException();
            }
        }
    }
}