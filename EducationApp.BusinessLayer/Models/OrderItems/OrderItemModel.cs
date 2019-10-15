using EducationApp.DataAccessLayer.Entities.Enums;

namespace EducationApp.BusinessLayer.Models.OrderItems
{
    public class OrderItemModel
    {
        public long PrintingEditionId { get; set; }
        public string Title { get; set; }
        public int Count { get; set; }
        public decimal Price { get; set; }
        public decimal Amount { get; set; }
        public Enums.Currency Currency { get; set; }
        public Enums.PrintingEditionType PrintingEditionType { get; set; }
    }
}
