using System;

namespace Whip.Services.Interfaces
{
    public interface IExceptionHandlingService
    {
        void Warn(Exception ex, string displayMessage = null);

        void Error(Exception ex, string displayMessage = null);

        void Fatal(Exception ex, string displayMessage = null);
    }
}
