using EducationApp.DataAccessLayer.Entities.Base;
using System;
using System.Collections.Generic;
using System.Text;

namespace EducationApp.DataAccessLayer.Entities
{
    public class PrintingEdition : BaseEntity
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public Enums.Enums.Currency Currency { get; set; }
        public Enums.Enums.PrintingEditionType PrintingEditionType { get; set; }
        public List<AuthorInPrintingEdition> AuthorInPrintingEditions { get; set; }
        public PrintingEdition()
        {
            AuthorInPrintingEditions = new List<AuthorInPrintingEdition>();
        }
    }
}
