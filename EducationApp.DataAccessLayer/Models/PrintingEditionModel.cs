using EducationApp.DataAccessLayer.Entities.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace EducationApp.DataAccessLayer.Models
{
    public class PrintingEditionModel
    {
        public string Title { get; set; }
        public Enums.PrintingEditionType PrintingEditionType { get; set; }

        public IList<object> AuthorsId { get; set; }
    }
}
