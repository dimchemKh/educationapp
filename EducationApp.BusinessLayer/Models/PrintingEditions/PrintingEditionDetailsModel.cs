using EducationApp.DataAccessLayer.Entities.Enums;

namespace EducationApp.BusinessLayer.Models.PrintingEditions
{
    public class PrintingEditionDetailsModel
    {
        public long Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public Enums.Currency Currency { get; set; }

    }
}
