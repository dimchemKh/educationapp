using Dapper.Contrib.Extensions;
using EducationApp.DataAccessLayer.Entities.Base;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace EducationApp.DataAccessLayer.Entities
{
    public class Author : BaseEntity
    {
        public string Name { get; set; }
        [Write(false)]
        public ICollection<AuthorInPrintingEdition> AuthorInPrintingEditions { get; set; }
        [Write(false)]
        [NotMapped]
        public ICollection<string> PrintingEditonTitles { get; set; }
        public Author()
        {
            AuthorInPrintingEditions = new List<AuthorInPrintingEdition>();
        }
    }
}
