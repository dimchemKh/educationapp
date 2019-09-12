using EducationApp.DataAccessLayer.Entities;
using EducationApp.DataAccessLayer.Entities.Base;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;

namespace EducationApp.DataAccessLayer.Repository.EFRepository
{
    public class UserRepository : Base.IBaseEFRepository<ApplicationUser>
    {
        public void Add(ApplicationUser entity) => throw new NotImplementedException();
        public void Delete(ApplicationUser entity) => throw new NotImplementedException();
        public void Edit(ApplicationUser entity) => throw new NotImplementedException();
        public ApplicationUser GetById(int id) => throw new NotImplementedException();
        public IEnumerable<ApplicationUser> List() => throw new NotImplementedException();
        public IEnumerable<ApplicationUser> List(Expression<Func<ApplicationUser, bool>> predicate) => throw new NotImplementedException();
    }
}
