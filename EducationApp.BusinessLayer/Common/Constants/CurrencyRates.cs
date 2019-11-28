using EducationApp.DataAccessLayer.Entities.Enums;
using System.Collections.Generic;

namespace EducationApp.BusinessLogic.Common.Constants
{
    public partial class Constants
    {
        public class CurrencyRates
        {
            public const decimal EURtoUSD = 1.09604m;
            public const decimal CHFtoUSD = 0.998971m;
            public const decimal GBPtoUSD = 1.23763m;
            public const decimal JPYtoUSD = 0.00933787m;
            public const decimal UAHtoUSD = 0.0403064m;
            public const decimal USDtoUSD = 1.0m;

            public static readonly Dictionary<Enums.Currency, decimal> ConverterDictionary = new Dictionary<Enums.Currency, decimal>()
            {
                { Enums.Currency.CHF, CHFtoUSD },
                { Enums.Currency.EUR, EURtoUSD },
                { Enums.Currency.GBP, GBPtoUSD },
                { Enums.Currency.JPY, JPYtoUSD },
                { Enums.Currency.UAH, UAHtoUSD },
                { Enums.Currency.USD, USDtoUSD }
            };
        }
    }
}
