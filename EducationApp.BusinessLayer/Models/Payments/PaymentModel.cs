using EducationApp.BusinessLayer.Models.Base;

namespace EducationApp.BusinessLayer.Models.Payments
{
    public class PaymentModel : BaseModel
    {
        public string TransactionId { get; set; }
        public long OrderId { get; set; }

    }
}
