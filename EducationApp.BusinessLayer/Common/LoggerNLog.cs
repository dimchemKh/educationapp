using EducationApp.BusinessLayer.Common.Interfaces;
using NLog;

namespace EducationApp.BusinessLayer.Common
{
    public class LoggerNLog : ILoggerNLog
    {
        private static readonly ILogger _logger = LogManager.GetCurrentClassLogger();
        public void Debug(string message)
        {
            _logger.Debug(message);
        }
        public void Error(string message)
        {
            _logger.Error(message);
        }
        public void Information(string message)
        {
            _logger.Info(message);
        }
        public void Warning(string message)
        {
            _logger.Warn(message);
        }
    }
}
