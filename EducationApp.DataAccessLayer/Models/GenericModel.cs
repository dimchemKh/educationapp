using System;
using System.Collections.Generic;
using System.Text;

namespace EducationApp.DataAccessLayer.Models
{
    public class GenericModel<T> where T : class
    {
        public IList<T> Collection = new List<T>();
        public int CollectionCount { get; set; }
    }
}
