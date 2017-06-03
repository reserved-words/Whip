using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Whip.Common
{
    public class HtmlAnchorHyperlinkPatternGroup : IHyperlinkPatternGroup
    {
        public HtmlAnchorHyperlinkPatternGroup()
        {
            Patterns = new List<HyperlinkPattern>
            {
                new HyperlinkPattern
                {
                    Pattern = @"<a\s+href=""(" + HyperlinkPattern.UrlPattern + @")""[^>]*>([^<]*)<\/a>",
                    Url = m => m.Groups[1].ToString(),
                    Text = m => m.Groups[2].ToString()
                }
            };
        }

        public List<HyperlinkPattern> Patterns { get; private set; }
    }
}
