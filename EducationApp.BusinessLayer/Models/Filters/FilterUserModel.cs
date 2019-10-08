using EducationApp.BusinessLayer.Models.Filters.Base;
using EducationApp.DataAccessLayer.Entities.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace EducationApp.BusinessLayer.Models.Filters
{
    public class FilterUserModel : BaseFilterModel
    {
        public string SearchByBody { get; set; }
        public ICollection<Enums.IsBlocked> Blocked { get; set; }
        public FilterUserModel() : base()
        {
            SearchByBody = null;
            Blocked = new List<Enums.IsBlocked>()
            {
                Enums.IsBlocked.False,
                Enums.IsBlocked.True
            };
        }
    }
}
