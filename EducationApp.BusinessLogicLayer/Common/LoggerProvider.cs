using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;

namespace EducationApp.BusinessLogicLayer.Common
{
    class LoggerProvider : ILoggerProvider
    {
        private string _path;
        public LoggerProvider(string path)
        {
            _path = path;
        }
        public ILogger CreateLogger(string categoryName)
        {
            return new Logger(_path);
        }
        public void Dispose()
        {

        }
    }
}
