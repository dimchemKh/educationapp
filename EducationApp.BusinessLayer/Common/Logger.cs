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
        public async void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception exception, Func<TState, Exception, string> formatter)
        {
            try
            {
                await WriteMessageAsync(logLevel, eventId, state, exception, formatter);
            }
            catch (Exception ex)
            {
                await WriteMessageAsync(logLevel, eventId, state, ex, formatter);
            }
        }
        private async Task WriteMessageAsync<TState>(LogLevel logLevel, EventId eventId, TState state, Exception exception, Func<TState, Exception, string> formatter)
        {
            if (!File.Exists(_filePath))
            {
                using (var stream = File.Create(_filePath))
                {
                    using (TextWriter tw = new StreamWriter(stream))
                    {
                        string message = $"{logLevel} :: {_categoryName} :: {formatter(state, exception)} :: Time => {DateTime.Now}";

                        await tw.WriteLineAsync(message);
                    }
                }
            }
        }
    }
}
