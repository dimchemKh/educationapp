using System;
using System.Collections.Generic;
using System.Text;

namespace EducationApp.DataAccessLayer.Models
{
    public class GenericModel<T> where T : class
    {
        public IEnumerable<T> Collection { get; set; }

        public int CollectionCount { get; set; }
    }
}
