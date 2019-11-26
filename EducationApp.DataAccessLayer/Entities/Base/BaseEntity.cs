using System;
using System.ComponentModel.DataAnnotations;

namespace EducationApp.DataAccessLayer.Entities.Base
{
    public class BaseEntity
    {
        [Key]
        public long Id { get; set; }
        public DateTime CreationDate { get; set; } = DateTime.Now;
        public bool IsRemoved { get; set; } = false;
    }
}
