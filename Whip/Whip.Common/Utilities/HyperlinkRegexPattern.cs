using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace Whip.Common.Utilities
{
    public enum HyperlinkRegexPatternType { HtmlAnchor, Tweet }

    public class HyperlinkRegexPattern
    {
        private const string UrlPattern = @"(?#Protocol)(?:(?:ht|f)tp(?:s?)\:\/\/|~/|/)?(?#Username:Password)(?:\w+:\w+@)?(?#Subdomains)(?:(?:[-\w]+\.)+(?#TopLevel Domains)(?:com|org|net|gov|mil|biz|info|mobi|name|aero|jobs|museum|travel|[a-z]{2}))(?#Port)(?::[\d]{1,5})?(?#Directories)(?:(?:(?:/(?:[-\w~!$+|.,=]|%[a-f\d]{2})+)+|/)+|\?|#)?(?#Query)(?:(?:\?(?:[-\w~!$+|.,*:]|%[a-f\d{2}])+=(?:[-\w~!$+|.,*:=]|%[a-f\d]{2})*)(?:&(?:[-\w~!$+|.,*:]|%[a-f\d{2}])+=(?:[-\w~!$+|.,*:=]|%[a-f\d]{2})*)*)*(?#Anchor)(?:#(?:[-\w~!$+|.,*:=]|%[a-f\d]{2})*)?";
        private const string TwitterHashTagUrlFormat = "https://twitter.com/hashtag/{0}";
        private const string TwitterUserUrlFormat = "https://twitter.com/{0}";

        public string Pattern { get; set; }

        public Func<Match, string> Url { get; set; }
        public Func<Match, string> Text { get; set; }

        public static List<HyperlinkRegexPattern> GetPatterns(HyperlinkRegexPatternType type)
        {
            switch (type)
            {
                case HyperlinkRegexPatternType.HtmlAnchor:
                    return new List<HyperlinkRegexPattern>
                        {
                            new HyperlinkRegexPattern
                            {
                                Pattern = @"<a\s+href=""(" + UrlPattern + @")""[^>]*>([^<]*)<\/a>",
                                Url = m => m.Groups[1].ToString(),
                                Text = m => m.Groups[2].ToString()
                            }
                        };
                case HyperlinkRegexPatternType.Tweet:
                    return new List<HyperlinkRegexPattern>
                        {
                            new HyperlinkRegexPattern
                            {
                                Pattern = UrlPattern,
                                Url = m => m.Groups[0].ToString(),
                                Text = m => m.Groups[0].ToString()
                            },
                            new HyperlinkRegexPattern
                            {
                                Pattern = @"\@([a-zA-Z0-9_]+)",
                                Url = m => string.Format(TwitterUserUrlFormat, m.Groups[1].ToString()),
                                Text = m => m.Groups[0].ToString()
                            },
                            new HyperlinkRegexPattern
                            {
                                Pattern = @"#([a-zA-Z0-9_]+)",
                                Url = m => string.Format(TwitterHashTagUrlFormat, m.Groups[1].ToString()),
                                Text = m => m.Groups[0].ToString()
                            }
                        };
                default:
                    throw new InvalidOperationException("Invalid hyperlink pattern type");
            }
        }
    }
}
