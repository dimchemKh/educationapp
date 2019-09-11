using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace EducationApp.BusinessLogicLayer.Common.Interfaces
{
    interface ILogger
    {
        void WriteMessage(string message);
    }
}
