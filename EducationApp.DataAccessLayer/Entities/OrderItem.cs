using EducationApp.DataAccessLayer.Entities.Base;

namespace EducationApp.DataAccessLayer.Entities
{
    public class OrderItem : BaseEntity
    {
        public int Amount { get; set; }
        public int Count { get; set; }
        public int PrintingEditionId { get; set; }
        public Enums.Enums.Currency Currency { get; set; }
        public PrintingEdition PrintingEdition { get; set; }
    }
}
