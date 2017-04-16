using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Whip.Services.Interfaces
{
    public interface IExceptionHandlingService
    {
        void Warn(Exception ex);

        void Error(Exception ex);

        void Fatal(Exception ex);
    }
}
