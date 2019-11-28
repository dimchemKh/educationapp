using System;
using System.Collections.Generic;
using System.Text;

namespace EducationApp.BusinessLogic.Models.Base
{
    public class BaseModel
    {
        public IList<string> Errors = new List<string>();
        public long ItemsCount { get; set; }
    }
}
