using EducationApp.DataAccessLayer.Entities.Enums;
using System.Collections.Generic;

namespace EducationApp.DataAccessLayer.Models.PrintingEditions
{
    public class DalPrintingEditionModel
    {
        public long Id { get; set; }
        public string Title { get; set; }
        public Enums.PrintingEditionType PrintingEditionType { get; set; }
        public Enums.Currency Currency { get; set; }
        public string Description { get; set; }
         
        public decimal Price { get; set; }
        public IList<string> AuthorNames { get; set; }
        public IList<long> AuthorsId { get; set; }
    }
}
