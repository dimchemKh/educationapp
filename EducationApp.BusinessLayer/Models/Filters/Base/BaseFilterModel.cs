using EducationApp.DataAccessLayer.Entities.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace EducationApp.BusinessLayer.Models.Filters.Base
{
    public class BaseFilterModel
    {
        public Enums.SortState SortState { get; set; }
        public int Page { get; set; }
        public Enums.SortType SortType{ get; set; }
        public Enums.PageSizes PageSize { get; set; }

        public BaseFilterModel()
        {
            SortState = Enums.SortState.Asc;
            Page = 1;
            SortType = Enums.SortType.Id;
            PageSize = Enums.PageSizes.Six;
        }
    }
}
