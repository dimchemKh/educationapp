using System;
using System.Collections.Generic;
using System.Text;

using System.ComponentModel.DataAnnotations;

namespace EducationApp.DataAccessLayer.Entities.Base
{
    public class BaseEntity
    {
        public virtual int Id { get; set; }
        public DateTime CreationDate { get; set; }
        public bool IsRemoved { get; set; }

    }
}
