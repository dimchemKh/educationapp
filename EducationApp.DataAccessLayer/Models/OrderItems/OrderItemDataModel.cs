using EducationApp.DataAccessLayer.Entities.Enums;

namespace EducationApp.DataAccessLayer.Models.OrderItems
{
    public class OrderItemDataModel
    {
        public string Title { get; set; }
        public int Count { get; set; }
        public decimal Amount { get; set; }
        public Enums.Currency Currency { get; set; }
        public Enums.PrintingEditionType PrintingEditionType { get; set; }
    }
}
