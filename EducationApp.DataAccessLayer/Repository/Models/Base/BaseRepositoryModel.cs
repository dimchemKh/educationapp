using EducationApp.DataAccessLayer.Entities.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace EducationApp.DataAccessLayer.Repository.Models.Base
{
    public class BaseRepositoryModel
    {
        public Enums.SortState SortState { get; set; }
        public Enums.SortType SortType { get; set; }
        public int PageSize { get; set; }
        public int Page { get; set; }
    }
}
