using EducationApp.BusinessLogic.Models.Authors;
using EducationApp.BusinessLogic.Models.Base;
using EducationApp.DataAccessLayer.Entities.Enums;
using System.Collections.Generic;

namespace EducationApp.BusinessLogic.Models.PrintingEditions
{
    public class PrintingEditionModelItem : BaseModel
    {
        public long Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public Enums.Currency Currency { get; set; }
        public Enums.PrintingEditionType PrintingEditionType { get; set; }
        public ICollection<AuthorModelItem> Authors { get; set; }
    }
}
