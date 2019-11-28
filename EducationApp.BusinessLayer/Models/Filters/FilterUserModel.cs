using EducationApp.BusinessLogic.Models.Filters.Base;
using EducationApp.DataAccessLayer.Entities.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace EducationApp.BusinessLogic.Models.Filters
{
    public class FilterUserModel : BaseFilterModel
    {
        public Enums.BlockState IsBlocked { get; set; }
    }
}
