using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Media3D;
using Whip.Services.Interfaces;

namespace Whip.Services
{
    public class TimeToString : ITimeToString
    {
        public string GetTimeAsTimeAgo(DateTime time, int maxMinutesAgo, bool excludeDateIfToday)
        {
            var currentTime = DateTime.Now;
            var timeAgo = currentTime - time;
            var minutesAgo = (int)Math.Round(timeAgo.TotalMinutes);

            if (minutesAgo <= maxMinutesAgo)
            {
                if (minutesAgo < 60)
                {
                    return $"{minutesAgo} minutes ago";
                }

                if (minutesAgo >= 60 && minutesAgo < 120)
                {
                    return "1 hour ago";
                }

                return $"{minutesAgo / 60} hours ago";
            }

            if (excludeDateIfToday && time.Date == currentTime.Date)
                return currentTime.ToShortTimeString();

            return currentTime.ToString();
        }
    }
}
