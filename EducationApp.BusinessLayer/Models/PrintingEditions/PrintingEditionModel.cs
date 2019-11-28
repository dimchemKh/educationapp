using EducationApp.BusinessLogic.Models.Base;
using System.Collections.Generic;

namespace EducationApp.BusinessLogic.Models.PrintingEditions
{
    public class PrintingEditionModel : BaseModel
    {
        public ICollection<PrintingEditionModelItem> Items = new List<PrintingEditionModelItem>();

    }
}
