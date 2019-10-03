using EducationApp.BusinessLayer.Models.Orders;
using EducationApp.BusinessLayer.Services;
using EducationApp.BusinessLayer.Services.Interfaces;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
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
        [HttpPost("myorders")]
        public async Task<IActionResult> GetUserOrdersAsync()
        {
            var responseModel = new OrderModel();
            var userId = User.Claims.First(id => id.Type == ClaimTypes.NameIdentifier)?.Value;

            return Ok(await _orderService.GetUserOrdersAsync(responseModel, userId));
        }
    }
}
