using EducationApp.DataAccessLayer.Entities;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;

namespace EducationApp.DataAccessLayer.Repository.EFRepository
{
    public class AuthorRepository : Base.IBaseEFRepository<Author>
    {
        public void Add(Author entity) => throw new NotImplementedException();
        public void Delete(Author entity) => throw new NotImplementedException();
        public void Edit(Author entity) => throw new NotImplementedException();
        public Author GetById(int id) => throw new NotImplementedException();
        public IEnumerable<Author> List() => throw new NotImplementedException();
        public IEnumerable<Author> List(Expression<Func<Author, bool>> predicate) => throw new NotImplementedException();
    }
}
