using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace EducationApp.BusinessLayer.Common
{
    public class Logger : ILogger //Interfaces.ILogger
    {
        //private string _filename = @"C:\Users\Anuitex-24\Documents\log.txt";

        //public async void WriteMessage(string message)
        //{
        //    using (FileStream file = File.Open(_filename, FileMode.OpenOrCreate))
        //    {
        //        await AddText(file, message);
        //    }
        //}
        //private Task AddText(FileStream fs, string value)
        //{
        //    byte[] info = new UTF8Encoding(true).GetBytes(value);
        //    return fs.WriteAsync(info, 0, info.Length);
        //}

        private string _filePath;
        private object _lock = new object();
        public Logger(string filePath)
        {
            _filePath = filePath;
        }

        public IDisposable BeginScope<TState>(TState state)
        {
            return null;
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
                   //File.AppendAllTextAsync(_filePath, formatter(state, exception) + Environment.NewLine);
                }
            }
        }
    }
}
