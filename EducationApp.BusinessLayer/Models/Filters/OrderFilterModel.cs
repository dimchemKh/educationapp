using EducationApp.BusinessLayer.Models.Base;
using EducationApp.BusinessLayer.Models.Filters.Base;
using EducationApp.DataAccessLayer.Entities.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace EducationApp.BusinessLayer.Models.Filters
{
    public class OrderFilterModel : BaseFilterModel
    {
        public ICollection<Enums.Status> Statuses { get; set; }

        public OrderFilterModel() : base()
        {
            
        }
    }
}
