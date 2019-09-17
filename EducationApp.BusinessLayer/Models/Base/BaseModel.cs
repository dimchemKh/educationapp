using System;
using System.Collections.Generic;
using System.Text;

namespace EducationApp.BusinessLayer.Models.Base
{
    public class BaseModel
    {
        public int Id { get; set; }
        public DateTime DateCreate { get; set; }
        public bool Remove { get; set; }
    }
}
