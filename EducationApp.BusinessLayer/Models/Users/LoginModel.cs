﻿using EducationApp.BusinessLayer.Models.Base;
using System;
using System.Collections.Generic;
using System.Text;

namespace EducationApp.BusinessLayer.Models.Users
{
    public class LoginModel : BaseModel
    {
        public ICollection<LoginModelItem> Items = new List<LoginModelItem>();
    }
}
