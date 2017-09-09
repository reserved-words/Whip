using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Whip.Services.Interfaces;

namespace Whip.Services
{
    public class CurrentDateTime : ICurrentDateTime
    {
        public DateTime Get()
        {
            return DateTime.Now;
        }
    }
}
