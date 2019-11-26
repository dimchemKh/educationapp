﻿using EducationApp.BusinessLayer.Common.Constants;
using EducationApp.DataAccessLayer.Entities.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace EducationApp.BusinessLayer.Models.Filters.Base
{
    public class BaseFilterModel
    {
        public string SearchString { get; set; }
        public Enums.SortState SortState { get; set; }
        public Enums.SortType SortType{ get; set; }
        public int PageSize { get; set; }
        public int Page { get; set; }
    }
}
