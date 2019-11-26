using Dapper.Contrib.Extensions;
using EducationApp.DataAccessLayer.Entities.Base;

namespace EducationApp.DataAccessLayer.Entities
{
    public class OrderItem : BaseEntity
    {
        public decimal Amount { get; set; }
        public int Count { get; set; }
        public Enums.Enums.Currency Currency { get; set; }
        public long PrintingEditionId { get; set; }
        [Write(false)]
        public PrintingEdition PrintingEdition { get; set; }
        public long OrderId { get; set; }
        [Write(false)]
        public Order Order { get; set; }
    }
}
