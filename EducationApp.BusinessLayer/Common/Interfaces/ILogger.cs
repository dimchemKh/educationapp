using System;
using System.Collections.Generic;
using System.Text;

namespace EducationApp.BusinessLayer.Common.Interfaces
{
    public interface ILogger
    {
        void WriteMessage(string message);
    }
}
