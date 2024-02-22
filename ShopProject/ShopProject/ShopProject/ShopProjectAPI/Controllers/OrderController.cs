using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Responsitory.DTO;
using Responsitory.IService;
using System.Threading.Tasks;
using System;
using Microsoft.AspNetCore.Authorization;
using ShopProjectAPI.Helper;

namespace ShopProjectAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class OrderController : ControllerBase
    {
        private readonly IOrderService _orderService;
        public OrderController(IOrderService orderService)
        {
            _orderService = orderService;
        }
        [Authorize(Policy = IdentifyRole.AdminRole)]
        [HttpGet]
        public async Task<IActionResult> GetOrder()
        {
            try
            {
                return Ok(await _orderService.GetOrderAsync());
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [Authorize]
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            try
            {
                var order = await _orderService.GetByIdAsync(id);
                return order == null ? NotFound() : Ok(order);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

        }

        [HttpPost]
        public async Task<IActionResult> CreateOrder(int id)
        {
            try
            {
                var newOrderId = await _orderService.CreateOrderAsync(id);
                var order = await _orderService.GetByIdAsync(newOrderId);
                return order == null ? NotFound() : Ok(order);
            }
            catch (Exception ex) 
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateOrder(int id, OrderResponse orderResponse)
        {
            try
            {
                await _orderService.UpdateOrderAsync(id, orderResponse);
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

        }
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteOrder(int id)
        {
            try
            {
                await _orderService.DeleteOrderAsync(id);
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

        }
        [HttpGet("Pluss")]
        public async Task<IActionResult> PlussOrderAsync(int id)
        {
            try
            {
                await _orderService.PlussOrderAsync(id);
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

        }
        [HttpGet("Sub")]
        public async Task<IActionResult> SubOrderAsync(int id)
        {
            try
            {
                await _orderService.SubOrderAsync(id);
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

        }
        [Authorize(Policy = IdentifyRole.AdminRole)]
        [HttpGet("CountOrder")]

        public async Task<IActionResult> Count()
        {
            try
            {
                CountOrder co = await _orderService.CountOr();
                return Ok(co);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);

            }

        }
        [HttpGet("TotalPriceOrder")]
        public async Task<IActionResult> Total()
        {
            try
            {
                OrderTotalPrice co = await _orderService.TotalPrice();
                return Ok(co);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);

            }

        }
        [HttpGet("OrderHistory")]
        public async Task<IActionResult> GetOrderHistoryAsync(int id)
        {
            try
            {
                var order = await _orderService.GetOrderHistoryAsync(id);
                return order == null ? NotFound() : Ok(order);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [Authorize(Policy = IdentifyRole.AdminRole)]
        [HttpGet("ConfirmOrder")]
        public async Task<IActionResult> ConfirmOrdeAsync(int id)
        {
            try
            {
                await _orderService.ConfirmOrdeAsync(id);
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
