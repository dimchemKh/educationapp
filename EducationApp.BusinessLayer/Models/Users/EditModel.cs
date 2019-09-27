using EducationApp.BusinessLayer.Models.Base;
using System;
using System.Collections.Generic;
using System.Text;

namespace EducationApp.BusinessLayer.Models.Users
{
    public class EditModel : BaseModel
    {
        public ICollection<EditModelItem> Items = new List<EditModelItem>();
    }
}
