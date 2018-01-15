using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Whip.Common.Exceptions
{
    public class WebServiceUnavailableException : Exception
    {
        public WebServiceUnavailableException(string message, Exception innerException)
            : base(message, innerException)
        {
        }
    }
}
