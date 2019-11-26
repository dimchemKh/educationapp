using EducationApp.BusinessLayer.Models.Base;
using System.Collections.Generic;

namespace EducationApp.BusinessLayer.Models.OrderItems
{
    public class OrderItemModel : BaseModel
    {
        public ICollection<OrderItemModelItem> Items = new List<OrderItemModelItem>();
    }
}
