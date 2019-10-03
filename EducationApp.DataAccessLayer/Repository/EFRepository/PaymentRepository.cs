using EducationApp.DataAccessLayer.AppContext;
using EducationApp.DataAccessLayer.Entities;
using EducationApp.DataAccessLayer.Repository.Base;
using System;
using System.Collections.Generic;
using System.Text;

namespace EducationApp.DataAccessLayer.Repository.EFRepository
{
    public class PaymentRepository : BaseEFRepository<Payment>
    {
        public PaymentRepository(ApplicationContext context) : base(context)
        {

        }
    }
}
