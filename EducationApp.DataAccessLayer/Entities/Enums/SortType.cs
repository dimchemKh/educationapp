using System;
using System.Collections.Generic;
using System.Text;

namespace EducationApp.DataAccessLayer.Entities.Enums
{
    public partial class Enums
    {
        public enum SortType
        {
            None = 0,
            Id = 1,
            Name = 2,
            Email = 3,
            TransactionStatus = 5,
            PrintingEditionType = 6,
            Price = 7
        }
    }
}
