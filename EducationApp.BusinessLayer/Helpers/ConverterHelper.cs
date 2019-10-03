using EducationApp.BusinessLayer.Helpers.Interfaces;
using System.Collections.Generic;
using EducationApp.DataAccessLayer.Entities.Enums;
using EducationApp.DataAccessLayer.Common.Constants;

namespace EducationApp.BusinessLayer.Helpers
{
    public class ConverterHelper : IConverterHelper
    {
        public static Dictionary<Enums.Currency, decimal> converterList = new Dictionary<Enums.Currency, decimal>()
            {
                { Enums.Currency.CHF, Constants.CurrencyRates.CHFtoUSD },
                { Enums.Currency.EUR, Constants.CurrencyRates.EURtoUSD },
                { Enums.Currency.GBP, Constants.CurrencyRates.GBPtoUSD },
                { Enums.Currency.JPY, Constants.CurrencyRates.JPYtoUSD },
                { Enums.Currency.UAH, Constants.CurrencyRates.UAHtoUSD },
                { Enums.Currency.USD, Constants.CurrencyRates.USDtoUSD }

            };
        public decimal Converting(Enums.Currency fromCurrency, Enums.Currency toCurrency, decimal sum)
        {
            
            decimal from = 0;
            decimal to = 0;
            foreach (var item in converterList)
            {
                if(item.Key == fromCurrency)
                {
                    from = item.Value;
                };
                if(item.Key == toCurrency)
                {
                    to = item.Value;
                };
            }
            return from / to * sum;
        }
    }    
}
