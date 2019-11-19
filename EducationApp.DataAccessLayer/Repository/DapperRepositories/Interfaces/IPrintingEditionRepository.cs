using EducationApp.DataAccessLayer.Entities;
using EducationApp.DataAccessLayer.Models;
using EducationApp.DataAccessLayer.Models.Filters;
using EducationApp.DataAccessLayer.Models.PrintingEditions;
using EducationApp.DataAccessLayer.Repository.Base.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace EducationApp.DataAccessLayer.Repository.DapperRepositories.Interfaces
{
    public interface IPrintingEditionRepository: IBaseDapperRepository<PrintingEdition>
    {
        Task<PrintingEditionDataModel> GetPrintingEditionDetailsAsync(long printingEditionId);
        Task<GenericModel<PrintingEditionDataModel>> GetPrintingEditionFilteredDataAsync(FilterPrintingEditionModel filter, bool isAdmin);
    }
}
