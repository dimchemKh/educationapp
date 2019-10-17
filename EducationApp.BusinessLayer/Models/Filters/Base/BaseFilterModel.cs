using EducationApp.DataAccessLayer.Entities.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace EducationApp.BusinessLayer.Models.Filters.Base
{
    public class BaseFilterModel
    {
        public Enums.SortState SortState { get; set; }
        public Enums.SortType SortType{ get; set; }
        public int PageSize { get; set; }
        public int Page { get; set; }

        public BaseFilterModel()
        {
            SortState = Enums.SortState.Asc;
            SortType = Enums.SortType.Id;            
            PageSize = (int)Enums.PageSize.Six;
            Page = 1;
        }
    }
}
