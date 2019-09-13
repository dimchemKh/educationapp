using EducationApp.DataAccessLayer.Entities;
using EducationApp.DataAccessLayer.Repository.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;

namespace EducationApp.DataAccessLayer.Repository.EFRepository
{
    class PrintingEditionRepository : IPrintingEditionRepository
    {
        public void Add(PrintingEdition entity) => throw new NotImplementedException();
        public void Delete(PrintingEdition entity) => throw new NotImplementedException();
        public void Edit(PrintingEdition entity) => throw new NotImplementedException();
        public PrintingEdition GetById(int id) => throw new NotImplementedException();
        public IEnumerable<PrintingEdition> GetPrintingEditions() => throw new NotImplementedException();
        public IEnumerable<PrintingEdition> List() => throw new NotImplementedException();
        public IEnumerable<PrintingEdition> List(Expression<Func<PrintingEdition, bool>> predicate) => throw new NotImplementedException();
        public void Save() => throw new NotImplementedException();
    }
}
