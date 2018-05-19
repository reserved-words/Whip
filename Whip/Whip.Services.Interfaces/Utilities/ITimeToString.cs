using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Whip.Services.Interfaces
{
    public interface ITimeToString
    {
        string GetTimeAsTimeAgo(DateTime time, int maxMinutesAgo, bool excludeDateIfToday);
    }
}
