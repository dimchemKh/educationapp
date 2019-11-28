using EducationApp.BusinessLogic.Models.Base;
using System;
using System.Collections.Generic;
using System.Text;

namespace EducationApp.BusinessLogic.Models.Authors
{
    public class AuthorModel : BaseModel
    {
        public ICollection<AuthorModelItem> Items = new List<AuthorModelItem>();
    }
}
