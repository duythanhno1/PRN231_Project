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
    public class OrderDetailController : ControllerBase
    {
        private readonly IOrderDetailService _orderdetailService;
        public OrderDetailController(IOrderDetailService orderdetailService)
        {
            _orderdetailService = orderdetailService;
        }
        //[Authorize(Policy = IdentifyRole.AdminRole)]
        [HttpGet]
        public async Task<IActionResult> GetOrderDetail(int id)
        {
            try
            {
                return Ok(await _orderdetailService.GetOrderDetailAdminAsync(id));
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            try
            {
                var orderdetail = await _orderdetailService.GetByIdAsync(id);
                return orderdetail == null ? NotFound() : Ok(orderdetail);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

        }

        [HttpPost]
        public async Task<IActionResult> CreateOrderDetail(OrderDetailResponse orderdetailResponse)
        {
            try
            {
                var newOrderDetailId = await _orderdetailService.CreateOrderDetailAsync(orderdetailResponse);
                var orderdetail = await _orderdetailService.GetByIdAsync(newOrderDetailId);
                return orderdetail == null ? NotFound() : Ok(orderdetail);
            }
            catch
            {
                return BadRequest();
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateOrderDetail(int id, OrderDetailResponse orderdetailResponse)
        {
            try
            {
                await _orderdetailService.UpdateOrderDetailAsync(id, orderdetailResponse);
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

        }
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteOrderDetail(int id)
        {
            try
            {
                await _orderdetailService.DeleteOrderDetailAsync(id);
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

        }
        [HttpGet("TotalPrice")]

        public async Task<IActionResult> TotalPrice(int id)
        {
            try
            {
                OrderDetailTotalPriceResponse co = await _orderdetailService.TotalPrice(id);
                return Ok(co);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);

            }
        }
    }
}
