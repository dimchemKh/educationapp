using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace EducationApp.BusinessLayer.Common.Interfaces
{
    public interface ILogger
    {
        void WriteMessage(string message);
    }
}
