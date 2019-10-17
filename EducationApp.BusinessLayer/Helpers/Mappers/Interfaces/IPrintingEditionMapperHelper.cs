using EducationApp.BusinessLayer.Models.PrintingEditions;
using EducationApp.DataAccessLayer.Models.PrintingEditions;
using System;
using System.Collections.Generic;
using System.Text;

namespace EducationApp.BusinessLayer.Helpers.Mappers.Interfaces
{
    public interface IPrintingEditionMapperHelper<MapFrom, MapTo> where MapFrom : PrintingEditionDataModel where MapTo : PrintingEditionModelItem, new()
    {
    }
}
