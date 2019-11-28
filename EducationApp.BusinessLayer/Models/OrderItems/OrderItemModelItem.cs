using EducationApp.BusinessLogic.Models.Base;
using EducationApp.DataAccessLayer.Entities.Enums;

namespace EducationApp.BusinessLogic.Models.OrderItems
{
    public class OrderItemModelItem : BaseModel
    {
        public long PrintingEditionId { get; set; }
        public string Title { get; set; }
        public int Count { get; set; }
        public decimal Price { get; set; }
        public Enums.Currency Currency { get; set; }
        public Enums.PrintingEditionType PrintingEditionType { get; set; }
    }
}
