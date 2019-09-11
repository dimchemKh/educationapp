using System;
using System.Collections.Generic;
using System.Text;

namespace EducationApp.DataAccessLayer.Entities
{
    public class AuthorInPrintingEdition
    {
        public Author AuthorId { get; set; }
        public PrintingEdition PrintingEditionId { get; set; }
        public DateTime Data { get; set; }

    }
}
