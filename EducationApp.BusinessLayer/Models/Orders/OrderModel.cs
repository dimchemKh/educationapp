using EducationApp.BusinessLogic.Models.Base;
using System.Collections.Generic;

namespace EducationApp.BusinessLogic.Models.Orders
{
    public class OrderModel : BaseModel
    {
        public ICollection<OrderModelItem> Items = new List<OrderModelItem>();
    }
}
