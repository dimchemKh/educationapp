using System;
using System.Collections.Generic;
using System.Text;

namespace EducationApp.DataAccessLayer.Entities.Base
{
    public abstract class BaseEntity
    {
        public abstract int Id { get; set; }
        public abstract DateTime CreationDate { get; set; }
        public abstract bool IsRemoved { get; set; }
            
        public BaseEntity()
        {
            CreationDate = DateTime.Now;
        }
    }
}
