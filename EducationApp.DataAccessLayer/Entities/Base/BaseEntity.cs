using System;
using System.Collections.Generic;
using System.Text;

using System.ComponentModel.DataAnnotations;

namespace EducationApp.DataAccessLayer.Entities.Base
{
    public class BaseEntity
    {
        public long Id { get; set; }
        public DateTime CreationDate { get; set; } = DateTime.Now;
        public bool IsRemoved { get; set; } = false;
    }
}
