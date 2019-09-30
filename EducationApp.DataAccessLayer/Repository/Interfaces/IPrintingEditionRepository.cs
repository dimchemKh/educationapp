﻿using EducationApp.DataAccessLayer.Entities;
using EducationApp.DataAccessLayer.Repository.Base.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace EducationApp.DataAccessLayer.Repository.Interfaces
{
    public interface IPrintingEditionRepository : IBaseEFRepository<PrintingEdition>
    {
        //Task<IQueryable<PrintingEdition>> GetPrintingEditionsAsync(Expression<Func<PrintingEdition, bool>> predicate1);
        Task<IQueryable<PrintingEdition>> GetIncludeAsync();

    }
}
