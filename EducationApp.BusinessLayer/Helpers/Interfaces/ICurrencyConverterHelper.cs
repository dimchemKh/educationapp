using EducationApp.DataAccessLayer.Entities.Enums;

namespace EducationApp.BusinessLogic.Helpers.Interfaces
{
    public interface ICurrencyConverterHelper
    {
        decimal Convert(Enums.Currency fromCurrency, Enums.Currency toCurrency, decimal sum);
    }
}
