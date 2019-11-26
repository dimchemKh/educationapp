using EducationApp.DataAccessLayer.AppContext;
using EducationApp.DataAccessLayer.Entities;
using EducationApp.DataAccessLayer.Repository.Base;
using EducationApp.DataAccessLayer.Repository.Interfaces;

namespace EducationApp.DataAccessLayer.Repository.EFRepository
{
    public class PaymentRepository : BaseEFRepository<Payment>, IPaymentRepository
    {
        public PaymentRepository(ApplicationContext context) : base(context)
        {
        }
        
    }
}
