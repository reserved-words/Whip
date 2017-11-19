using NLog;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Whip.Services.Interfaces;

namespace Whip.NLog
{
    public class LoggingService : ILoggingService
    {
        private static Logger logger = LogManager.GetCurrentClassLogger();

        public void Trace(string message)
        {
            logger.Trace(message);
        }

        public void Debug(string message)
        {
            logger.Debug(message);
        }

        public void Info(string message)
        {
            logger.Info(message);
        }

        public void Warn(string message)
        {
            logger.Warn(message);
        }

        public void Error(string message)
        {
            logger.Error(message);
        }

        public void Fatal(string message)
        {
            logger.Fatal(message);
        }
    }
}
