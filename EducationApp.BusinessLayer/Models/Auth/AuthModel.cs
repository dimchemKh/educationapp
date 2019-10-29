﻿using EducationApp.BusinessLayer.Models.Base;
using EducationApp.BusinessLayer.Models.Users;
using System.Collections.Generic;
using System.Net;

namespace EducationApp.BusinessLayer.Models.Auth
{
    public class AuthModel : BaseModel
    {
        public string AccessToken { get; set; }
        public string RefreshToken { get; set; }
    }
}
