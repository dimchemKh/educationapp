using System;
using System.Collections.Generic;
using System.Text;

namespace EducationApp.DataAccessLayer.Entities.Enums
{
    public partial class Enums
    {
        public enum StateSort
        {
            None = 0,
            PriceAsc = 1,
            PriceDesc = 2,
            IdAsc = 3,
            IdDesc = 4,
            BookAsc = 5,
            BookDesc = 6
        }
    }
}
