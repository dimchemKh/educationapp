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
using EducationApp.BusinessLayer.Models.Helpers;

namespace EducationApp.BusinessLayer.Helpers
{
    public class ConverterHelper : IConverterHelper
    {
        public IEnumerable<PrintingEdition> GetFilteringListAsync(IEnumerable<PrintingEdition> printingEditions, UserFilterModel userFilterModel)
        {
            List<PrintingEdition> responseList = new List<PrintingEdition>();

            var listToUSD = new Dictionary<Enums.Currency, decimal>()
            {
                { Enums.Currency.CHF, Constants.CurrencyRates.CHFtoUSD },
                { Enums.Currency.EUR, Constants.CurrencyRates.EURtoUSD },
                { Enums.Currency.GBP, Constants.CurrencyRates.GBPtoUSD },
                { Enums.Currency.JPY, Constants.CurrencyRates.JPYtoUSD },
                { Enums.Currency.UAH, Constants.CurrencyRates.UAHtoUSD },
                { Enums.Currency.USD, Constants.CurrencyRates.USDtoUSD }
            };
            var listToFilterCurrency = new Dictionary<Enums.Currency, decimal>()
            {
                { Enums.Currency.CHF, Constants.CurrencyRates.USDToCHF },
                { Enums.Currency.EUR, Constants.CurrencyRates.USDtoEUR },
                { Enums.Currency.GBP, Constants.CurrencyRates.USDtoGBP },
                { Enums.Currency.JPY, Constants.CurrencyRates.USDtoJPY },
                { Enums.Currency.UAH, Constants.CurrencyRates.USDtoUAH },
                { Enums.Currency.USD, Constants.CurrencyRates.USDtoUSD }
            };
            foreach (var rate in listToUSD)
            {
                responseList.AddRange(printingEditions.Where(x => x.Currency == rate.Key)
                    .Select(z => new PrintingEdition()
                    {
                        Id = z.Id,
                        Name = z.Name,
                        Type = z.Type,
                        Price = z.Price * rate.Value,
                        AuthorInPrintingEdition = z.AuthorInPrintingEdition
                    }).ToList());
            }
            foreach (var rate in listToFilterCurrency)
            {
                if(userFilterModel.Currency == rate.Key)
                {
                    responseList = responseList.Select(z => new PrintingEdition()
                    {
                        Id = z.Id,
                        Name = z.Name,
                        Type = z.Type,
                        Price = z.Price * rate.Value,
                        AuthorInPrintingEdition = z.AuthorInPrintingEdition
                    }).ToList();
                }
            }
            return responseList;
        }
    }
}
