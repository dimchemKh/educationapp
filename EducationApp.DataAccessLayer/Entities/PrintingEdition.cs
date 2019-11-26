using Dapper.Contrib.Extensions;
using EducationApp.DataAccessLayer.Entities.Base;
using System.Collections.Generic;

namespace EducationApp.DataAccessLayer.Entities
{
    [Table("PrintingEditions")]
    public class PrintingEdition : BaseEntity
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public Enums.Enums.Currency Currency { get; set; }
        public Enums.Enums.PrintingEditionType PrintingEditionType { get; set; }
        [Write(false)]
        public ICollection<AuthorInPrintingEdition> AuthorInPrintingEditions { get; set; }
        public PrintingEdition()
        {
            AuthorInPrintingEditions = new List<AuthorInPrintingEdition>();
        }
    }
}
