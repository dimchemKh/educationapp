using EducationApp.DataAccessLayer.Entities.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace EducationApp.BusinessLayer.Models.PrintingEditions
{
    public class AdminFilterModel
    {
        public ICollection<Enums.Type> Types { get; set; }
        public Enums.StateSort StateSort { get; set; }
        public int Page { get; set; }

        public AdminFilterModel()
        {
            Types = new List<Enums.Type>();
            StateSort = Enums.StateSort.PriceDesc;
        }
    }
}
