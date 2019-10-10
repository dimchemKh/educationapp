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
        public async Task<IActionResult> GetUserOrdersAsync()
        {
            var responseModel = new OrderModel();
            var userId = User.Claims.First(id => id.Type == ClaimTypes.NameIdentifier)?.Value;
            //await _orderService.GetUserOrdersAsync(responseModel, userId);
            return Ok();
        }
        [HttpPost("usersOrders")]
        public async Task<IActionResult> GetUsersOrdersForAdminAsync([FromBody]FilterOrderModel orderFilterModel)
        {
            var responseModel = new OrderModel();
            if (!User.Claims.First(role => role.Type == ClaimTypes.Role).Value.Contains(Constants.Roles.Admin))
            {
                responseModel.Errors.Add(Constants.Errors.InvalidToken);
                return Ok(responseModel);
            }
                       
            // TODO: admin orders

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
