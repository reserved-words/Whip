using System;
using System.Text.RegularExpressions;

namespace Whip.Common
{
    public enum HyperlinkPatternType { HtmlAnchor, Tweet }

    public class HyperlinkPattern
    {
        public static string UrlPattern = @"(?#Protocol)(?:(?:ht|f)tp(?:s?)\:\/\/|~/|/)?(?#Username:Password)(?:\w+:\w+@)?(?#Subdomains)(?:(?:[-\w]+\.)+(?#TopLevel Domains)(?:com|org|net|gov|mil|biz|info|mobi|name|aero|jobs|museum|travel|[a-z]{2}))(?#Port)(?::[\d]{1,5})?(?#Directories)(?:(?:(?:/(?:[-\w~!$+|.,=]|%[a-f\d]{2})+)+|/)+|\?|#)?(?#Query)(?:(?:\?(?:[-\w~!$+|.,*:]|%[a-f\d{2}])+=(?:[-\w~!$+|.,*:=]|%[a-f\d]{2})*)(?:&(?:[-\w~!$+|.,*:]|%[a-f\d{2}])+=(?:[-\w~!$+|.,*:=]|%[a-f\d]{2})*)*)*(?#Anchor)(?:#(?:[-\w~!$+|.,*:=]|%[a-f\d]{2})*)?";
        
        public string Pattern { get; set; }
        public Func<Match, string> Url { get; set; }
        public Func<Match, string> Text { get; set; }

        public static IHyperlinkPatternGroup GetPatternGroup(HyperlinkPatternType type)
        {
            switch (type)
            {
                case HyperlinkPatternType.HtmlAnchor:
                    return new HtmlAnchorHyperlinkPatternGroup();
                case HyperlinkPatternType.Tweet:
                    return new TweetHyperlinkPatternGroup();
                default:
                    throw new InvalidOperationException("Invalid hyperlink pattern type");
            }
        }
    }
}
