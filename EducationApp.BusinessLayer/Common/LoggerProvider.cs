using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace EducationApp.BusinessLayer.Common
{
    public class LoggerProvider : ILoggerProvider
    {
        private readonly string _filePath;
        public LoggerProvider(string filePath)
        {
            _filePath = filePath;
        }

        public ILogger CreateLogger(string categoryName)
        {
            return new Logger(categoryName, _filePath);
        }

        public void Dispose()
        {

        }
    }
}
