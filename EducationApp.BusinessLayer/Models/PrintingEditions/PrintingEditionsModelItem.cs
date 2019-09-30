using EducationApp.BusinessLayer.Models.Authors;
using EducationApp.DataAccessLayer.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace EducationApp.BusinessLayer.Models.PrintingEditions
{
    public class PrintingEditionsModelItem
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Author { get; set; }
        public decimal Price { get; set; }

        public ICollection<Author> AuthorModels { get; set; }
    }
}
