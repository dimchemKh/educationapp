using EducationApp.DataAccessLayer.Entities.Enums;
using EducationApp.DataAccessLayer.Models.Authors;
using System.Collections.Generic;

namespace EducationApp.DataAccessLayer.Models.PrintingEditions
{
    public class PrintingEditionDataModel
    {
        public long Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public Enums.Currency Currency { get; set; }
        public Enums.PrintingEditionType PrintingEditionType { get; set; }
        public ICollection<AuthorDataModel> Authors { get; set; }
        
    }
}
