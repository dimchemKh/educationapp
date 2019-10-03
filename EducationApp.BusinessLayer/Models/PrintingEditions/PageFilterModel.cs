using EducationApp.DataAccessLayer.Entities.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace EducationApp.BusinessLayer.Models.PrintingEditions
{
    public class PageFilterModel
    {
        public int Id { get; set; }
        public Enums.Currency Currency { get; set; }
    }
}
