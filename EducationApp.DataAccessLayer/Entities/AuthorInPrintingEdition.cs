using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace EducationApp.DataAccessLayer.Entities
{
    public class AuthorInPrintingEdition
    {
        public int Id { get; set; }
        public Author AuthorId { get; set; }
        public PrintingEdition PrintingEditionId { get; set; }
        public DateTime Data { get; set; }

    }
}
