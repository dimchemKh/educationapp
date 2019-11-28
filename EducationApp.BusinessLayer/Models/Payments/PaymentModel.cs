using EducationApp.BusinessLogic.Models.Base;

namespace EducationApp.BusinessLogic.Models.Payments
{
    public class PaymentModel : BaseModel
    {
        public string TransactionId { get; set; }
        public long OrderId { get; set; }

    }
}
