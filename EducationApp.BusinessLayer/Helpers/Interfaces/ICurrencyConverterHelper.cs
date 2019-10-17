using EducationApp.BusinessLayer.Models.PrintingEditions;
using EducationApp.DataAccessLayer.Entities;
using EducationApp.DataAccessLayer.Entities.Enums;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace EducationApp.BusinessLayer.Helpers.Interfaces
{
    public interface ICurrencyConverterHelper
    {
        decimal Converting(Enums.Currency fromCurrency, Enums.Currency toCurrency, decimal sum);
    }
}
