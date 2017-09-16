using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Tweetinvi;
using Tweetinvi.Models;
using Whip.Common.Model;
using Whip.Services.Interfaces;
using Whip.Services.Interfaces.Singletons;
using static Whip.Common.Resources;

namespace Whip.TweetInvi
{
    public class TwitterService : ITwitterService
    {
        private readonly IConfigSettings _configSettings;

        public TwitterService(IConfigSettings configSettings)
        {
            _configSettings = configSettings;
        }

        public async Task<bool> PopulateTweets(Artist artist)
        {
            if (string.IsNullOrEmpty(artist.Twitter))
                return true;

            var tweets = await Task.Run(() => GetTweets(artist.Twitter, 20));

            foreach (var t in tweets)
            {
                var image = string.Empty;

                var media = t.Media.FirstOrDefault();

                if (media != null && media.MediaType == "photo")
                {
                    image = media.MediaURL;
                }

                var tweetForInfo = t.IsRetweet ? t.RetweetedTweet : t;

                var screenName = tweetForInfo.CreatedBy.ScreenName;
                var name = tweetForInfo.CreatedBy.Name;
                var url = tweetForInfo.Url;
                var userImage = tweetForInfo.CreatedBy.ProfileImageUrl;

                var tweet = new Common.Model.Tweet
                {
                    Posted = t.CreatedAt,
                    Username = $"@{screenName}",
                    Name = name,
                    Url = url,
                    Content = t.FullText,
                    UserImage = userImage,
                    Image = image,
                    IsRetweet = t.IsRetweet,
                    UserUrl = string.Format(TwitterUserUrlFormat, screenName)
                };

                artist.Tweets.Add(tweet);
            }

            return true;
        }

        private List<ITweet> GetTweets(string username, int maxTweets)
        {
            var credentials = Auth.SetUserCredentials(
                _configSettings.TwitterApiKey,
                _configSettings.TwitterApiSecret,
                _configSettings.TwitterApiAccessToken,
                _configSettings.TwitterApiAccessTokenSecret);

            var tweets = Auth.ExecuteOperationWithCredentials(credentials, () =>
            {
                return Timeline.GetUserTimeline(username, maxTweets)?.ToList();
            });

            return tweets;
        }
    }
}
