using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace EducationApp.BusinessLayer.Common
{
    public class Logger : ILogger 
    {
        private readonly string _filePath;
        private readonly string _categoryName;
        public Logger(string categoryName, string filePath)
        {
            _filePath = filePath;
            _categoryName = categoryName;
        }

        public IDisposable BeginScope<TState>(TState state)
        {
            return null;
        }
        public bool IsEnabled(LogLevel logLevel)
        {
            return logLevel != LogLevel.None;
        }
        public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception exception, Func<TState, Exception, string> formatter)
        {
            try
            {
                WriteMessageAsync(logLevel, eventId, state, exception, formatter);
            }
            catch (Exception ex)
            {
                WriteMessageAsync(logLevel, eventId, state, ex, formatter);
            }
        }
        private void WriteMessageAsync<TState>(LogLevel logLevel, EventId eventId, TState state, Exception exception, Func<TState, Exception, string> formatter)
        {
            if (File.Exists(_filePath))
            {

                    using (StreamWriter tw = new StreamWriter(_filePath))
                    {
                        string message = $"{logLevel} :: {_categoryName} :: {formatter(state, exception)} :: Time => {DateTime.Now}";

                        tw.WriteLine(message);
                    }

            }
        }
    }
}
