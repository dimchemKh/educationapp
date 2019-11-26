using EducationApp.BusinessLayer.Helpers.Interfaces;
using EducationApp.DataAccessLayer.Entities.Enums;
using EducationApp.BusinessLayer.Common.Constants;

namespace EducationApp.BusinessLayer.Helpers
{
    public class CurrencyConverterHelper : ICurrencyConverterHelper
    {
        public decimal Convert(Enums.Currency fromCurrency, Enums.Currency toCurrency, decimal result)
        {
            decimal valueFrom = 0;
            decimal valueTo = 0;
            foreach (var item in Constants.CurrencyRates.ConverterDictionary)
            {
                if(item.Key == fromCurrency)
                {
                    valueFrom = item.Value;
                };
                if(item.Key == toCurrency)
                {
                    valueTo = item.Value;
                };
            }
            return valueFrom / valueTo * result;
        }
    }    
}
