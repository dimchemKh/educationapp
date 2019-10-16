using System;
using System.Collections.Generic;
using System.Text;

namespace EducationApp.DataAccessLayer.Models.PrintingEditions
{
    public class PrintingEditionShortDataModel
    {
        public long Id { get; set; }
        public ICollection<long> AuthorsId { get; set; }
    }
}
