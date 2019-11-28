using EducationApp.BusinessLogic.Models.Base;
using EducationApp.BusinessLogic.Models.Filters.Base;
using EducationApp.DataAccessLayer.Entities.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace EducationApp.BusinessLogic.Models.Filters
{
    public class FilterOrderModel : BaseFilterModel
    {
        
        public Enums.TransactionStatus TransactionStatus { get; set; }
    }
}
