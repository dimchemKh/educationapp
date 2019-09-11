using System;
using System.Collections.Generic;
using System.Text;

namespace EducationApp.DataAccessLayer.Entities
{
    public class OrderItem
    {
        public int Id { get; set; }
        public int Amount { get; set; }
        public int Currency { get; set; }
        public PrintingEdition PrintingEditionId { get; set; }
        public int Count { get; set; }
    }
}
