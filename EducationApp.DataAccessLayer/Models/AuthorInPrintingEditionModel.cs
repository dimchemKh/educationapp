using System;
using System.Collections.Generic;
using System.Text;

namespace EducationApp.DataAccessLayer.Models
{
    public class AuthorInPrintingEditionModel
    {
        public long AuthorId { get; set; }
        public ICollection<string> PrintingEditionTitle { get; set; }
        public string AuthorName { get; set; }

    }
}
