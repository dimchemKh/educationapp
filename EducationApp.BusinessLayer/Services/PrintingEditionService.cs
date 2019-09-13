using EducationApp.BusinessLayer.Services.Interfaces;
using EducationApp.DataAccessLayer.Repository.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace EducationApp.BusinessLayer.Services
{
    public class PrintingEditionService : IPrintingEditionService
    {
        private IPrintingEditionRepository _printingEditionRepository;
        public PrintingEditionService(IPrintingEditionRepository printingEditionRepository)
        {
            _printingEditionRepository = printingEditionRepository;
        }
    }
}
