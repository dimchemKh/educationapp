using EducationApp.DataAccessLayer.Entities.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace EducationApp.BusinessLayer.Models.Authors
{
    public class AuthorFilterModel
    {
        public Enums.StateSort StateSort { get; set; }
        public int Page { get; set; }
    }
}
