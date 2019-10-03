using EducationApp.BusinessLayer.Helpers.Interfaces;
using EducationApp.BusinessLayer.Models.PrintingEditions;
using EducationApp.DataAccessLayer.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using System.Threading.Tasks;
using EducationApp.DataAccessLayer.Entities.Enums;
using EducationApp.DataAccessLayer.Common.Constants;

namespace EducationApp.BusinessLayer.Helpers
{
    public class ConverterHelper : IConverterHelper
    {
        public decimal Converting(Enums.Currency fromCurrency, Enums.Currency toCurrency, decimal sum)
        {
            //var EUR_JPY = Constants.CurrencyRates.EURtoUSD / Constants.CurrencyRates.JPYtoUSD * sum;
            var list = new Dictionary<Enums.Currency, decimal>()
            {
                { Enums.Currency.CHF, 0.998971m },
                { Enums.Currency.EUR, 1.09604m },
                { Enums.Currency.GBP, 1.23763m },
                { Enums.Currency.JPY, 0.00933787m },
                { Enums.Currency.UAH, 0.0403064m },
                { Enums.Currency.USD, 1.0m }

            };

            decimal from = 0;
            decimal to = 0;

            foreach (var item in list)
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
