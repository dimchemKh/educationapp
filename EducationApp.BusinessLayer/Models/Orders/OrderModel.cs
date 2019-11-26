using EducationApp.BusinessLayer.Models.Base;
using System.Collections.Generic;

namespace EducationApp.BusinessLayer.Models.Orders
{
    public class OrderModel : BaseModel
    {
        public ICollection<OrderModelItem> Items = new List<OrderModelItem>();
    }
}
