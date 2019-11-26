using EducationApp.DataAccessLayer.Entities.Enums;

namespace EducationApp.BusinessLayer.Helpers.Interfaces
{
    public interface ICurrencyConverterHelper
    {
        decimal Convert(Enums.Currency fromCurrency, Enums.Currency toCurrency, decimal sum);
    }
}
