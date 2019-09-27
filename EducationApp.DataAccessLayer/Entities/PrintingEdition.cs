using EducationApp.DataAccessLayer.Entities.Base;
using System;
using System.Collections.Generic;
using System.Text;

namespace EducationApp.DataAccessLayer.Entities
{
    public class PrintingEdition : BaseEntity
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public ICollection<decimal> Currency { get; set; } = new List<decimal>();
        public Enums.Enums.Type Type { get; set; }
        public ICollection<AuthorInBooks> AuthorInBooks { get; set; } = new List<AuthorInBooks>();
    }
}
