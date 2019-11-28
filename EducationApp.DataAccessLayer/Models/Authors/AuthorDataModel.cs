using System.Collections.Generic;

namespace EducationApp.DataAccessLayer.Models.Authors
{
    public class AuthorDataModel
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public ICollection<string> PrintingEditionTitles { get; set; }
    }
}
