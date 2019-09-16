using EducationApp.DataAccessLayer.Entities.Base;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace EducationApp.DataAccessLayer.Entities
{
    public class Author : BaseEntity
    {
        public new int Id { get; set; }
        public new DateTime CreationDate { get; set; }
        public new bool IsRemoved { get; set; }
        public string Name { get; set; }
    }
}
