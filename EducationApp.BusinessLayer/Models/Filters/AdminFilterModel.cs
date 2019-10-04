using EducationApp.BusinessLayer.Models.Base;
using EducationApp.BusinessLayer.Models.Filters.Base;
using EducationApp.DataAccessLayer.Entities.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace EducationApp.BusinessLayer.Models.Filters
{
    public class AdminFilterModel : BaseFilterModel
    {
        public ICollection<Enums.Type> Types { get; set; }
        public AdminFilterModel() : base()
        {
            Types = new List<Enums.Type>();
            SortState = Enums.SortState.Asc;
        }
    }
}
