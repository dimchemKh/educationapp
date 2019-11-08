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
            Price = 4,
            TransactionStatus = 5,
            PrintingEditionType = 6,
            Title = 7,
            FirstName = 8
        }
    }
}
