using System.Collections.Generic;

namespace EducationApp.DataAccessLayer.Models
{
    public class GenericModel<T> where T : class
    {
        public IEnumerable<T> Collection = new List<T>();
        public int CollectionCount { get; set; }
    }
}
