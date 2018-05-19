using System;
using System.Text;
using Whip.Services.Interfaces;
using Whip.Services.Interfaces.Singletons;

namespace Whip.Services
{
    public class ErrorLoggingService : IErrorLoggingService
    {
        private const string DataSeparator = " | ";
        private const string DataKeyValueSeparator = " = ";

        private readonly IConfigSettings _configSettings;
        private readonly IWebHelperService _webHelperService;

        public ErrorLoggingService(IWebHelperService webHelperService, IConfigSettings configSettings)
        {
            _configSettings = configSettings;
            _webHelperService = webHelperService;
        }

        public void Log(Exception ex)
        {
            var log = new
            {
                ApplicationName = "Whip",
                Data = GetData(ex),
                Message = ex.Message,
                StackTrace = ex.StackTrace
            };

            _webHelperService.HttpPostAsync(_configSettings.LoggingUrl, log);
        }

        private static string GetData(Exception ex)
        {
            var data = new StringBuilder();
            var separatorLength = DataSeparator.Length;

            foreach (var key in ex.Data.Keys)
            {
                data.Append(key);
                data.Append(DataKeyValueSeparator);
                data.Append(ex.Data[key]);
                data.Append(DataSeparator);
            }

            if (data.Length > 0)
            {
                data.Remove(data.Length - separatorLength, separatorLength);
            }

            return data.ToString();
        }
    }


}
