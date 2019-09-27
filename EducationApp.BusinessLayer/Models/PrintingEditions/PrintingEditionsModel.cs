using EducationApp.BusinessLayer.Models.Base;
using System;
using System.Collections.Generic;
using System.Text;

namespace EducationApp.BusinessLayer.Models.PrintingEditions
{
    public class PrintingEditionsModel : BaseModel
    {
        public ICollection<PrintingEditionsModelItem> Items = new List<PrintingEditionsModelItem>();

    }
}
