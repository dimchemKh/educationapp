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
        [HttpPost("get")]
        public async Task<IActionResult> GetOrdersAsync(string role, [FromBody]FilterOrderModel filterOrder)
        {
            var userId = string.Empty;

            if(!string.IsNullOrWhiteSpace(role) && role.Equals(Constants.Roles.Admin))
            {
                userId = Constants.AdminSettings.AdminId.ToString();
            }

            if(!string.IsNullOrWhiteSpace(role) && role.Equals(Constants.Roles.User))
            {
                userId = User.Claims.FirstOrDefault(x => x.Type.Equals(ClaimTypes.NameIdentifier))?.Value;
            }

            var responseModel = await _orderService.GetOrdersAsync(filterOrder, userId);

            return Ok(responseModel);
        }
        [Authorize(Roles = Constants.Roles.User)]
        [HttpPost("create")]
        public async Task<IActionResult> CreateOrderAsync(string role, [FromBody]OrderModelItem orderModelItem)
        {
            var userId = string.Empty;

            if (!string.IsNullOrWhiteSpace(role) && role.Equals(Constants.Roles.User))
            {
                userId = User.Claims.FirstOrDefault(x => x.Type.Equals(ClaimTypes.NameIdentifier))?.Value;
            }

            var responseModel = await _orderService.CreateOrderAsync(orderModelItem, userId);

            return Ok(responseModel);
        }        
        [HttpPost("update/{orderId}")]
        public async Task<IActionResult> UdpdateOrderAsync(string orderId)
        {
            var responseModel = await _orderService.CreateTransactionAsync(orderId, "2132333323232");

            return Ok(responseModel);
        }
    }
}
