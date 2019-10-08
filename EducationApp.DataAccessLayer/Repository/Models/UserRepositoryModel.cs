using EducationApp.DataAccessLayer.Entities.Enums;
using EducationApp.DataAccessLayer.Repository.Models.Base;
using System;
using System.Collections.Generic;
using System.Text;

namespace EducationApp.DataAccessLayer.Repository.Models
{
    public class UserRepositoryModel : BaseRepositoryModel
    {
        public string SearchByBody { get; set; }
        public ICollection<Enums.IsBlocked> Blocked { get; set; }
    }
}
