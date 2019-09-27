using EducationApp.BusinessLayer.Models.Authors;
using System;
using System.Collections.Generic;
using System.Text;

namespace EducationApp.BusinessLayer.Models.PrintingEditions
{
    public class PrintingEditionsModelItem
    {
        public string Name { get; set; }
        public string Author { get; set; }
        public decimal Price { get; set; }

        public ICollection<AuthorModelItem> AuthorModels { get; set; }
    }
}
