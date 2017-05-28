using System;
using System.Collections.Generic;

namespace Whip.Common.Utilities
{
    public enum HyperlinkRegexPatternType { HtmlAnchor }

    public class HyperlinkRegexPattern
    {
        private const string UrlPattern = @"(?#Protocol)(?:(?:ht|f)tp(?:s?)\:\/\/|~/|/)?(?#Username:Password)(?:\w+:\w+@)?(?#Subdomains)(?:(?:[-\w]+\.)+(?#TopLevel Domains)(?:com|org|net|gov|mil|biz|info|mobi|name|aero|jobs|museum|travel|[a-z]{2}))(?#Port)(?::[\d]{1,5})?(?#Directories)(?:(?:(?:/(?:[-\w~!$+|.,=]|%[a-f\d]{2})+)+|/)+|\?|#)?(?#Query)(?:(?:\?(?:[-\w~!$+|.,*:]|%[a-f\d{2}])+=(?:[-\w~!$+|.,*:=]|%[a-f\d]{2})*)(?:&(?:[-\w~!$+|.,*:]|%[a-f\d{2}])+=(?:[-\w~!$+|.,*:=]|%[a-f\d]{2})*)*)*(?#Anchor)(?:#(?:[-\w~!$+|.,*:=]|%[a-f\d]{2})*)?";

        public string Pattern { get; set; }
        public int MatchUrlGroupIndex { get; set; }
        public int MatchTextGroupIndex { get; set; }

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
                                MatchUrlGroupIndex = 1,
                                MatchTextGroupIndex = 2
                            }
                        };
                default:
                    throw new InvalidOperationException("Invalid hyperlink pattern type");
            }
        }
    }
}
