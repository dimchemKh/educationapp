using EducationApp.DataAccessLayer.Entities.Enums;
using System.Collections.Generic;

namespace EducationApp.BusinessLayer.Models.OrderItems
{
    public class OrderItemModel
    {
        public ICollection<OrderItemModelItem> Items = new List<OrderItemModelItem>();
    }
}
