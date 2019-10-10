using System.Collections.Generic;

namespace EducationApp.BusinessLayer.Models.Authors
{
    public class AuthorModelItem
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public ICollection<string> PrintingEditionTitles { get; set; }
    }
}