using EducationApp.DataAccessLayer.Entities;
using EducationApp.DataAccessLayer.Repository.Base;
using EducationApp.DataAccessLayer.Repository.Interfaces;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Text;

namespace EducationApp.DataAccessLayer.Repository.DapperRepositories
{
    public class PaymentRepository : BaseDapperRepository<Payment>, IPaymentRepository
    {
        public PaymentRepository(IConfiguration configuration) : base(configuration)
        {

        }
    }
}
