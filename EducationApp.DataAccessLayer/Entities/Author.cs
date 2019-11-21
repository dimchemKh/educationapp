using Dapper.Contrib.Extensions;
using EducationApp.DataAccessLayer.Entities.Base;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace EducationApp.DataAccessLayer.Entities
{
    public class Author : BaseEntity
    {
        public string Name { get; set; }
        [Write(false)]
        public ICollection<AuthorInPrintingEdition> AuthorInPrintingEditions { get; set; }
        [NotMapped]
        public ICollection<string> PrintingEditonTitles { get; set; }
        public Author()
        {
            AuthorInPrintingEditions = new List<AuthorInPrintingEdition>();
        }
    }
}
