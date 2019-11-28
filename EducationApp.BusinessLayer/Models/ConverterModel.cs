using EducationApp.DataAccessLayer.Entities.Enums;

namespace EducationApp.BusinessLogic.Models
{
    public class ConverterModel
    {
        public Enums.Currency CurrencyFrom { get; set; }
        public Enums.Currency CurrencyTo { get; set; }
        public decimal Price { get; set; }
    }
}
