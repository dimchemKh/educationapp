using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace EducationApp.DataAccessLayer.Entities
{
    public class AuthorInPrintingEdition
    {
        public int Id { get; set; }
        public Author AuthorsId { get; set; }
        public PrintingEdition PrintingEditionsId { get; set; }
        public DateTime Data { get; set; }

    }
}
