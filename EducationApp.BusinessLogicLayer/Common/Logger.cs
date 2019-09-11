using Microsoft.Extensions.Logging;
using System;
using System.IO;

namespace EducationApp.BusinessLogicLayer.Common
{
    public class Logger : ILogger
    {
        private string _filepath;
        private object _lock = new object();
        public Logger(string filepath)
        {
            _filepath = filepath;
        }

        public bool IsEnabled(LogLevel logLevel)
        {
            return true;
        }
        public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception exception, Func<TState, Exception, string> formatter)
        {
            if(formatter != null)
            {
                lock (_lock)
                {
                    File.AppendAllText(_filepath, formatter(state, exception) + Environment.NewLine);
                }
            }
        }
        public IDisposable BeginScope<TState>(TState state)
        {
            return null;
        }
    }
}
