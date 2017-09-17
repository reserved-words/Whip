using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Whip.Common.Model
{
    public class Log
    {
        public int Id { get; set; }
        public DateTime LoggedAt { get; set; }
        public string Type { get; set; }
        public string Message { get; set; }
    }
}
