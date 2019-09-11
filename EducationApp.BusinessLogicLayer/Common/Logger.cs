using Microsoft.Extensions.Logging;
using System;
using System.IO;

namespace EducationApp.BusinessLogicLayer.Common
{
    public class Logger : Interfaces.ILogger
    {
        public readonly ILogger<Logger> _logger;

        public Logger(ILogger<Logger> logger)
        {
            _logger = logger;
            
        }
        public void WriteMessage(string message)
        {
            _logger.LogInformation(message);

            using (StreamWriter writer = new StreamWriter("Logging.txt", false))
            {
                writer.WriteLine("Log: \r\n");
                writer.WriteLine($"{DateTime.Now.ToLongTimeString()}");
                writer.WriteLine($"{message}");
                writer.WriteLine("------------------------------");
            }
        }
    }
}
