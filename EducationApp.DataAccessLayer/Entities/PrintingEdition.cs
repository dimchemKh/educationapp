using System;
using System.Collections.Generic;
using System.Text;

namespace EducationApp.DataAccessLayer.Entities
{
    public class PrintingEdition
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int Price { get; set; }

        public bool IsRemoved { get; set; }
        public Enums.Enums.Status Status { get; set; }
        public Enums.Enums.Currency Currency { get; set; }
        public Enums.Enums.Type Type { get; set; }

    }
}
