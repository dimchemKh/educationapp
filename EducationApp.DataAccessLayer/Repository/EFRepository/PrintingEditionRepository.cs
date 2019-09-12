using EducationApp.DataAccessLayer.Entities;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;

namespace EducationApp.DataAccessLayer.Repository.EFRepository
{
    class PrintingEditionRepository : Base.IBaseEFRepository<PrintingEdition>
    {
        public void Add(PrintingEdition entity)
        {

        }
        public void Delete(PrintingEdition entity)
        {

        }
        public void Edit(PrintingEdition entity)
        {

        }
        public PrintingEdition GetById(int id)
        {
            return null;
        }
        public IEnumerable<PrintingEdition> List()
        {
            return null;
        }
        public IEnumerable<PrintingEdition> List(Expression<Func<PrintingEdition, bool>> predicate)
        {
            return null;
        }
    }
}
