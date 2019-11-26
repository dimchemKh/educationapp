using EducationApp.DataAccessLayer.Entities;
using EducationApp.DataAccessLayer.Repository.Base;
using EducationApp.DataAccessLayer.Repository.Interfaces;
using Microsoft.Extensions.Configuration;

namespace EducationApp.DataAccessLayer.Repository.DapperRepositories
{
    public class PaymentRepository : BaseDapperRepository<Payment>, IPaymentRepository
    {
        public PaymentRepository(IConfiguration configuration) : base(configuration)
        {

        }
    }
}
