using EducationApp.DataAccessLayer.AppContext;
using EducationApp.DataAccessLayer.Entities;
using EducationApp.DataAccessLayer.Repository.Base;
using EducationApp.DataAccessLayer.Repository.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace EducationApp.DataAccessLayer.Repository.EFRepository
{
    public class AuthorInBooksRepository : BaseEFRepository<AuthorInBooks>, IAuthorInBooksRepository
    {
        public AuthorInBooksRepository(ApplicationContext context) : base(context)
        {
        }
    }
}
