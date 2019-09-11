using System;
using System.Collections.Generic;
using System.Text;

namespace EducationApp.DataAccessLayer.Entities
{
    public class PrintingEdition : Base.BaseEntity
    {
        public override int Id { get; set; }
        public override bool IsRemoved { get; set; }
        public override DateTime CreationDate { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int Price { get; set; }
        public int Status { get; set; }
        public int Currency { get; set; }
        public Enums.Enums.BookType Type { get; set; }
    }
}
