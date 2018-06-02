using System;
using System.Collections.Generic;
using Whip.Services.Interfaces;
using Whip.Services.Interfaces.Singletons;

namespace Whip.XmlDataAccess
{
    public class ConfigSettings : IConfigSettings
    {
        private readonly IXmlProvider _xmlProvider;

        private bool _populated;

        public ConfigSettings(IXmlProvider xmlProvider)
        {
            _xmlProvider = xmlProvider;

            PopulateSettings();
        }

        public string ApplicationName { get; private set; }
        public string LastFmApiKey { get; private set; }
        public string LastFmApiSecret { get; private set; }
        public string BandsInTownApiKey { get; private set; }
        public string YouTubeApiKey { get; private set; }
        public List<string> FileExtensions { get; private set; }
        public int TrackChangeDelay { get; private set; }
        public int MinutesBeforeRefreshNews { get; private set; }
        public int NumberOfSimilarArtistsToDisplay { get; private set; }
        public int DaysBeforeUpdatingArtistWebInfo { get; private set; }
        public int MinutesBeforeUpdatingTweets { get; private set; }
        public string TwitterApiKey { get; private set; }
        public string TwitterApiSecret { get; private set; }
        public string TwitterApiAccessToken { get; private set; }
        public string TwitterApiAccessTokenSecret { get; private set; }
        public string LoggingUrl { get; private set; }

        private void PopulateSettings()
        {
            if (_populated)
                return;

            var config = _xmlProvider.Get();
            var root = config.Element("config");

            ApplicationName = root.Element("application_name").Value;
            TrackChangeDelay = Convert.ToInt16(root.Element("track_change_delay").Value);
            NumberOfSimilarArtistsToDisplay = Convert.ToInt16(root.Element("num_similar_artists").Value);
            MinutesBeforeRefreshNews = Convert.ToInt16(root.Element("minutes_before_update_news").Value);
            DaysBeforeUpdatingArtistWebInfo = Convert.ToInt16(root.Element("days_before_update_web_info").Value);
            MinutesBeforeUpdatingTweets = Convert.ToInt16(root.Element("minutes_before_update_tweets").Value);
            LoggingUrl = root.Element("logging_url").Value;

            FileExtensions = new List<string>();
            foreach (var fileExtension in root.Element("file_extensions").Elements("file_extension"))
            {
                FileExtensions.Add(fileExtension.Value);
            }

            foreach (var apiSetting in root.Element("api_settings").Elements("api_setting"))
            {
                switch (apiSetting.Attribute("name").Value)
                {
                    case "Last.fm":
                        LastFmApiKey = apiSetting.Attribute("api_key").Value;
                        LastFmApiSecret = apiSetting.Attribute("secret").Value;
                        break;
                    case "BandsInTown":
                        BandsInTownApiKey = apiSetting.Attribute("api_key").Value;
                        break;
                    case "YouTube":
                        YouTubeApiKey = apiSetting.Attribute("api_key").Value;
                        break;
                    case "Twitter":
                        TwitterApiKey = apiSetting.Attribute("api_key").Value;
                        TwitterApiSecret = apiSetting.Attribute("secret").Value;
                        TwitterApiAccessToken = apiSetting.Attribute("token").Value;
                        TwitterApiAccessTokenSecret = apiSetting.Attribute("token_secret").Value;
                        break;
                    default:
                        break;
                }
            }

            _populated = true;
        }
    }
}
