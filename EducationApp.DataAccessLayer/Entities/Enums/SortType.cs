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
            Block = 3,
            Book = 4,
            Type = 5,
            Price = 6,
            Status = 7
        }
    }
}
