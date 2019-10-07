using System.Collections.Generic;

namespace EducationApp.BusinessLayer.Models.Authors
{
    public class AuthorModelItem
    {
        public long Id { get; set; }
        public string AuthorName { get; set; }
        public ICollection<string> PrintingEditionsTitles { get; set; }
    }
}