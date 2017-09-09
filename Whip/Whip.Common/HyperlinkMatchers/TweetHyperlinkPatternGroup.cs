using System.Collections.Generic;
using static Whip.Common.Resources;

namespace Whip.Common
{
    public class TweetHyperlinkPatternGroup : IHyperlinkPatternGroup
    {
        public TweetHyperlinkPatternGroup()
        {
            Patterns = new List<HyperlinkPattern>
            {
                new HyperlinkPattern
                {
                    Pattern = HyperlinkPattern.UrlPattern,
                    Url = m => m.Groups[0].ToString(),
                    Text = m => m.Groups[0].ToString()
                },
                new HyperlinkPattern
                {
                    Pattern = @"\@([a-zA-Z0-9_]+)",
                    Url = m => string.Format(TwitterUserUrlFormat, m.Groups[1].ToString()),
                    Text = m => m.Groups[0].ToString()
                },
                new HyperlinkPattern
                {
                    Pattern = @"#([a-zA-Z0-9_]+)",
                    Url = m => string.Format(TwitterHashTagUrlFormat, m.Groups[1].ToString()),
                    Text = m => m.Groups[0].ToString()
                }
            };
        }

        public List<HyperlinkPattern> Patterns { get; private set; }
    }
}
