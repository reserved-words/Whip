using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Whip.ViewModels.Messages
{
    public class SkipToPercentageMessage
    {
        public SkipToPercentageMessage(double newPercentage)
        {
            NewPercentage = newPercentage;
        }

        public double NewPercentage { get; private set; }
    }
}
