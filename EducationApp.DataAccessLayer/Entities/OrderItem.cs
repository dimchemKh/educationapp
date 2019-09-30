using EducationApp.DataAccessLayer.Entities.Base;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace EducationApp.DataAccessLayer.Entities
{
    public class OrderItem : BaseEntity
    {
        public int Amount { get; set; }
        public Enums.Enums.Currency Currency { get; set; }
        public int Count { get; set; }

        public int PrintingEditionId { get; set; }
        [ForeignKey("PrintingEditionId")]
        public PrintingEdition PrintingEdition { get; set; }
        
    }
}
