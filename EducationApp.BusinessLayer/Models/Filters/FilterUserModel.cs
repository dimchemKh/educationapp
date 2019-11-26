﻿using EducationApp.BusinessLayer.Models.Filters.Base;
using EducationApp.DataAccessLayer.Entities.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace EducationApp.BusinessLayer.Models.Filters
{
    public class FilterUserModel : BaseFilterModel
    {
        public Enums.BlockState IsBlocked { get; set; }
    }
}
