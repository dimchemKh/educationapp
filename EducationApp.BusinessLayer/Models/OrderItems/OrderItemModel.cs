using EducationApp.BusinessLogic.Models.Base;
using System.Collections.Generic;

namespace EducationApp.BusinessLogic.Models.OrderItems
{
    public class OrderItemModel : BaseModel
    {
        public ICollection<OrderItemModelItem> Items = new List<OrderItemModelItem>();
    }
}
