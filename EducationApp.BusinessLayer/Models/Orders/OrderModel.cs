using EducationApp.BusinessLayer.Models.Base;
using System;
using System.Collections.Generic;
using System.Text;

namespace EducationApp.BusinessLayer.Models.Orders
{
    public class OrderModel : BaseModel
    {
        public ICollection<OrderModelItem> Items = new List<OrderModelItem>();
    }
}
