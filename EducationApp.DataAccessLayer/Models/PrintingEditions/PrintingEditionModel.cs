using EducationApp.DataAccessLayer.Entities.Enums;
using EducationApp.DataAccessLayer.Models.Base;
using System;
using System.Collections.Generic;
using System.Text;

namespace EducationApp.DataAccessLayer.Models.PrintingEditions
{
    public class PrintingEditionModel : BaseModel
    {
        public long Id { get; set; }
        public string Title { get; set; }
        public Enums.PrintingEditionType PrintingEditionType { get; set; }
        public Enums.Currency Currency { get; set; }

        public decimal Price { get; set; }
        public IList<string> AuthorNames { get; set; }
        public IList<long> AuthorsId { get; set; }
    }
}
