using System.Collections.Generic;

namespace EducationApp.BusinessLayer.Models.Authors
{
    public class AuthorModelItem
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public ICollection<string> ProductTitles { get; set; }
    }
}