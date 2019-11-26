using EducationApp.BusinessLayer.Models.Base;
using System.Collections.Generic;

namespace EducationApp.BusinessLayer.Models.PrintingEditions
{
    public class PrintingEditionModel : BaseModel
    {
        public ICollection<PrintingEditionModelItem> Items = new List<PrintingEditionModelItem>();

    }
}
