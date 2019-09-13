using EducationApp.DataAccessLayer.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace EducationApp.DataAccessLayer.Repository.Interfaces
{
    public interface IPrintingEditionRepository
    {
        IEnumerable<PrintingEdition> GetPrintingEditions();
    }
}
