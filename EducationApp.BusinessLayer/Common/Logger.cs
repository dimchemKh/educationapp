using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace EducationApp.BusinessLayer.Common
{
    public class Logger : Interfaces.ILogger
    {
        private string _filename = @"C:\Users\Anuitex-24\Documents\log.txt";
       
        public async void WriteMessage(string message)
        {
            using (FileStream file = File.Open(_filename, FileMode.OpenOrCreate))
            {
                await AddText(file, message);
            }
        }
        private Task AddText(FileStream fs, string value)
        {
            byte[] info = new UTF8Encoding(true).GetBytes(value);
            return fs.WriteAsync(info, 0, info.Length);
        }
    }
}
