using EducationApp.DataAccessLayer.Entities.Base;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace EducationApp.DataAccessLayer.Entities
{
    public class Author : BaseEntity
    {
        public string Name { get; set; }
        public ICollection<AuthorInPrintingEdition> AuthorInPrintingEditions { get; set; }
        public Author()
        {
            AuthorInPrintingEditions = new List<AuthorInPrintingEdition>();
        }
    }
}
