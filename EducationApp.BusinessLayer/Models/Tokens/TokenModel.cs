using EducationApp.BusinessLayer.Models.Base;
using EducationApp.BusinessLayer.Models.Users;
using System.Collections.Generic;

namespace EducationApp.BusinessLayer.Models.Tokens
{
    public class TokenModel : BaseModel
    {
        public string AccessToken { get; set; }
        public string RefreshToken { get; set; }
    }
}
