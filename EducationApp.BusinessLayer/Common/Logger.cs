using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace EducationApp.BusinessLayer.Common
{
    public class Logger : Interfaces.ILogger
    {
        public void WriteMessage(string message)
        {
            using(StreamWriter sw = new StreamWriter("logging.txt"))
            {
                sw.WriteLine($"Log: Message - {message}");
            }
        }
    }
}
