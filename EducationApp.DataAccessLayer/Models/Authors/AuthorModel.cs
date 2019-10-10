using System;
using System.Collections.Generic;
using System.Text;

namespace EducationApp.DataAccessLayer.Models.Authors
{
    public class AuthorModel
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public ICollection<string> PrintingEditionTitles { get; set; }

    }
}
