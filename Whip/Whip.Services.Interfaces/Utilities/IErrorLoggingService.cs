using System;

namespace Whip.Services.Interfaces
{
    public interface IErrorLoggingService
    {
        void Log(Exception ex);
    }
}
