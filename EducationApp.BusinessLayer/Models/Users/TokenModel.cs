using EducationApp.BusinessLayer.Models.Base;
using EducationApp.BusinessLayer.Models.Users;
using System.Collections.Generic;

namespace EducationApp.BusinessLayer.Models.Users
{
    public class TokenModel : BaseModel
    {
        public ICollection<TokenModeltem> Items = new List<TokenModeltem>();
    }
}
