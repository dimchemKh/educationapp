using EducationApp.DataAccessLayer.Entities.Base;
using System;
using System.Collections.Generic;
using System.Text;

namespace EducationApp.DataAccessLayer.Entities
{
    public class OrderItem : BaseEntity
    {
        public int Amount { get; set; }
        public Enums.Enums.Currency Currency { get; set; }
        public int Count { get; set; }
        public PrintingEdition PrintingEditionsId { get; set; }
        
    }
}
