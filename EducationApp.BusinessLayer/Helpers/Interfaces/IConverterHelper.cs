using EducationApp.BusinessLayer.Models.Helpers;
using EducationApp.BusinessLayer.Models.PrintingEditions;
using EducationApp.DataAccessLayer.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace EducationApp.BusinessLayer.Helpers.Interfaces
{
    public interface IConverterHelper
    {
        IEnumerable<PrintingEdition> GetFilteringListAsync(IEnumerable<PrintingEdition> printingEditions, UserFilterModel userFilterModel);
    }
}
