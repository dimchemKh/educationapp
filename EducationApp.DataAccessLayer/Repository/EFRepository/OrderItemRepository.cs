using EducationApp.DataAccessLayer.AppContext;
using EducationApp.DataAccessLayer.Entities;
using EducationApp.DataAccessLayer.Repository.Base;
using EducationApp.DataAccessLayer.Repository.EFRepository.Interfaces;

namespace EducationApp.DataAccessLayer.Repository.EFRepository
{
    public class OrderItemRepository : BaseEFRepository<OrderItem>, IOrderItemRepository
    {
        public OrderItemRepository(ApplicationContext context) : base(context)
        {

        }
    }
}
