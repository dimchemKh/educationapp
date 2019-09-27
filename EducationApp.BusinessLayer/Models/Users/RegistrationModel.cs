using EducationApp.BusinessLayer.Models.Base;
using System;
using System.Collections.Generic;
using System.Text;

namespace EducationApp.BusinessLayer.Models.Users
{
    public class RegistrationModel : BaseModel
    {
        public ICollection<RegistrationModelItem> Items = new List<RegistrationModelItem>();

    }
}
