using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Whip.Common.Exceptions
{
    public class WebServiceMethodCallException : Exception
    {
        public WebServiceMethodCallException(string message, Exception innerException)
            : base(message, innerException)
        {
        }
    }
}
