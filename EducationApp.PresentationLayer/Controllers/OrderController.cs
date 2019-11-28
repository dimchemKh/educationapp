using EducationApp.BusinessLogic.Models;
using EducationApp.BusinessLogic.Models.Filters;
using EducationApp.BusinessLogic.Models.Orders;
using EducationApp.BusinessLogic.Models.Payments;
using EducationApp.BusinessLogic.Services.Interfaces;
using EducationApp.DataAccessLayer.Common.Constants;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace EducationApp.Presentation.Controllers
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
        [HttpPost("converting")]
        public async Task<IActionResult> ConvertPriceAsync([FromBody] ConverterModel converterModel)
        {
            var resultConverting = await _orderService.ConvertPriceAsync(converterModel);
            return Ok(resultConverting);
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
        public async Task<IActionResult> CreateOrderAsync(string role, [FromBody] OrderModelItem orderModelItem)
        {
            var userId = string.Empty;

            if (!string.IsNullOrWhiteSpace(role) && role.Equals(Constants.Roles.User))
            {
                userId = User.Claims.FirstOrDefault(x => x.Type.Equals(ClaimTypes.NameIdentifier))?.Value;
            }

            var responseModel = await _orderService.CreateOrderAsync(orderModelItem, userId);

            return Ok(responseModel);
        }
        [HttpPost("update")]
        public async Task<IActionResult> UpdateOrderTransactionAsync([FromBody] PaymentModel payment)
        {
            var responseModel = await _orderService.CreateTransactionAsync(payment);

            return Ok(responseModel);
        }
    }
}
