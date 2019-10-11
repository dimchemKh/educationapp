using EducationApp.DataAccessLayer.Entities.Enums;
using EducationApp.DataAccessLayer.Models.Filters.Base;
using System;
using System.Collections.Generic;
using System.Text;

namespace EducationApp.DataAccessLayer.Models.Filters
{
    public class FilterOrderModel : BaseFilterModel
    {
        public Enums.TransactionStatus TransactionStatus { get; set; }
    }
}
