using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Whip.Common
{
    public interface IHyperlinkPatternGroup
    {
        List<HyperlinkPattern> Patterns { get; }
    }
}
