using System;
using System.Collections.Generic;
using System.Text;

namespace EducationApp.DataAccessLayer.Models
{
    public class GenericModel<T> where T : class
    {
        public List<T> Collection = new List<T>();
        public int CollectionCount { get; set; }
    }
}
