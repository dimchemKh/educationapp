﻿using System;
using System.Collections.Generic;
using System.Text;

namespace EducationApp.BusinessLayer.Models
{
    public class TokenModel
    {
        public string AccessToken { get; set; }
        public string RefreshToken{ get; set; }
    }
}
