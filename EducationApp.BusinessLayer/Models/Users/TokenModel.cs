using System;
using System.Collections.Generic;
using System.Text;

namespace EducationApp.BusinessLayer.Models
{
    public class TokenModel
    {
        public string AccessToken { get; private set; }
        public string RefreshToken{ get; private set; }

        public TokenModel(string accessToken, string refreshToken)
        {
            AccessToken = accessToken;
            RefreshToken = refreshToken;
        }
    }
}
