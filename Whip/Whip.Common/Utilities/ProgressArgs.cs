using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Whip.Common.Utilities
{
    public class ProgressArgs
    {
        public ProgressArgs(int percentage, string message)
        {
            Percentage = percentage;
            Message = message;
        }

        public int Percentage { get; set; }
        public string Message { get; set; }
    }
}
