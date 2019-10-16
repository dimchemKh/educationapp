using EducationApp.BusinessLayer.Models.Authors;
using EducationApp.DataAccessLayer.Entities;
using EducationApp.DataAccessLayer.Entities.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace EducationApp.BusinessLayer.Models.PrintingEditions
{
    public class PrintingEditionModelItem
    {
        public long Id { get; set; }
        public string Title { get; set; }
        public Enums.PrintingEditionType PrintingEditionType { get; set; }
        public Enums.Currency Currency { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }        
        public IList<AuthorModelItem> Authors { get; set; }
    }
}
