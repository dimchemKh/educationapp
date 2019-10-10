using System;
using System.Collections.Generic;
using System.Text;

namespace EducationApp.DataAccessLayer.Models
{
    public class PrintingEditionsInAuthorModel
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public ICollection<string> PrintingEditionTitles { get; set; }
        public ICollection<long> PrintingEditionsId { get; set; }
    }
}
