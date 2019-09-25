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
        private string _filePath;
        private string _categoryName;
        private object _lock = new object();
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
                WriteMessage(logLevel, eventId, state, exception, formatter);
            }
            catch (Exception)
            {
                WriteMessage(logLevel, eventId, state, exception, formatter);
            }
        }
        private void WriteMessage<TState>(LogLevel logLevel, EventId eventId, TState state, Exception exception, Func<TState, Exception, string> formatter)
        {
            string message = $"{logLevel} :: {_categoryName} :: {formatter(state, exception)} :: Time => {DateTime.Now}";

            using (var writer = File.AppendText(_filePath))
            {
                writer.WriteLine(message);
            }
        }
    }
}
