using EducationApp.BusinessLogic.Models.Base;
using System.Collections.Generic;

namespace EducationApp.BusinessLogic.Models.Authors
{
    public class AuthorModelItem : BaseModel
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public ICollection<string> PrintingEditionTitles { get; set; }
    }
}