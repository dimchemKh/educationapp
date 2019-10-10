using System;
using System.Collections.Generic;
using System.Text;

namespace EducationApp.DataAccessLayer.Models
{
    public class AuthorsInPrintingEditionModel
    {

        public long Id { get; set; }
        public string Title { get; set; }
        public ICollection<long> AuthorsId { get; set; }
        public ICollection<string> AuthorsNames { get; set; }

    }
}
