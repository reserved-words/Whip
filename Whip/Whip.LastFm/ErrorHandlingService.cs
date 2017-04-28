using System;
using LastFmApi;
using Whip.Services.Interfaces;

namespace Whip.LastFm
{
    public class ErrorHandlingService : IErrorHandlingService
    {
        private readonly ILoggingService _logger;

        public ErrorHandlingService(ILoggingService logger)
        {
            _logger = logger;
        }

        public void HandleError(LastFmApiException ex, string additionalInfo = "")
        {
            switch (ex.ErrorCode)
            {
                case ErrorCode.InvalidParameters:
                    _logger.Warn(FormatErrorMessage(ex, additionalInfo));
                    return;
            }
        }

        private string FormatErrorMessage(LastFmApiException ex, string additionalInfo)
        {
            return string.Format("Last.FM {0} : {1}{2}", 
                ex.ErrorCode?.ToString() ?? string.Empty, 
                ex.Message, 
                string.IsNullOrEmpty(additionalInfo) ? string.Empty : $" ({additionalInfo})");
        }
    }
}
