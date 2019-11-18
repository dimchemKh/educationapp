using EducationApp.DataAccessLayer.Entities;
using EducationApp.DataAccessLayer.Repository.Base;
using EducationApp.DataAccessLayer.Repository.DapperRepositories.Interfaces;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Text;

namespace EducationApp.DataAccessLayer.Repository.DapperRepositories
{
    public class PrintingEditionRepository : BaseDapperRepository<PrintingEdition>, IPrintingEditionRepository
    {
        public PrintingEditionRepository(IConfiguration configuration) : base(configuration)
        {

        }
    }
}
