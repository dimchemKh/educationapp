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
        public async Task<IActionResult> GetUserOrdersAsync([FromBody]FilterOrderModel filterOrder)
        {
            var userId = User.Claims.First(x => x.Type == ClaimTypes.NameIdentifier).Value;
            var responseModel = await _orderService.GetUserOrdersAsync(filterOrder, userId);
            return Ok(responseModel);
        }
        [HttpPost("create")]
        public async Task<IActionResult> CreateOrderAsync([FromBody]OrderModelItem orderModelItem)
        {
            var userId = User.Claims.First(x => x.Type == ClaimTypes.NameIdentifier).Value;
            var responseModel = await _orderService.CreateOrderAsync(orderModelItem, userId);
            return Ok(responseModel);
        }
    }
}
