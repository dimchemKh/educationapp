using EducationApp.BusinessLayer.Models.Filters;
using EducationApp.BusinessLayer.Models.Orders;
using EducationApp.BusinessLayer.Services.Interfaces;
using EducationApp.DataAccessLayer.Common.Constants;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace EducationApp.PresentationLayer.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class OrderController : Controller
    {
        private readonly IOrderService _orderService;
        public OrderController(IOrderService orderService)
        {
            _orderService = orderService;
        }
        [HttpPost("myOrders")]
        public async Task<IActionResult> GetUserOrdersAsync([FromBody]FilterOrderModel filterOrder)
        {
            var userId = User.Claims.First(x => x.Type == ClaimTypes.NameIdentifier).Value;
            var role = User.Claims.First(x => x.Type == ClaimTypes.Role).Value;
            var responseModel = await _orderService.GetUserOrdersAsync(filterOrder, userId, role);
            return Ok(responseModel);
        }
        [HttpPost("myOrders/transaction")]
        public async Task<IActionResult> CreateTransaction([FromBody] long orderId, long transactionId)
        {
            var userId = User.Claims.First(x => x.Type == ClaimTypes.NameIdentifier).Value;
            var responseModel = await _orderService.CreateTransactionAsync(orderId, transactionId);
            return Ok(responseModel);
        }
        [HttpPost("createOrder")]
        public async Task<IActionResult> CreateOrderAsync([FromBody]OrderModelItem orderModelItem)
        {
            var userId = User.Claims.First(x => x.Type == ClaimTypes.NameIdentifier).Value;
            var responseModel = await _orderService.CreateOrderAsync(orderModelItem, long.Parse(userId));
            return Ok(responseModel);
        }
    }
}
