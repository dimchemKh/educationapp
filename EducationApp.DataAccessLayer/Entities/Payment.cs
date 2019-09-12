using EducationApp.DataAccessLayer.Entities.Base;
using System;
using System.Collections.Generic;
using System.Text;

namespace EducationApp.DataAccessLayer.Entities
{
    public class Payment : BaseEntity
    {
        public int Id { get; set; }
        public int TransactionId { get; set; }

    }
}
