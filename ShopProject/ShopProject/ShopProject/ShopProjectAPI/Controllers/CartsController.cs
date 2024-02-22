using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Responsitory.IService;
using Responsitory.DTO;
using System.Collections.Generic;
using Microsoft.AspNetCore.Authorization;
using ShopProjectAPI.Helper;

namespace ShopProjectAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class CartsController : ControllerBase
    {
        private readonly ICartService _cartService;
        public CartsController(ICartService cartService)
        {
            _cartService = cartService;
        }

        [HttpGet("Cart")]
        public async Task<IActionResult> GetCart(int id)
        {
            try
            {
                var check = await _cartService.GetCartsAsync(id);

                if (check == null)
                {
                    return NotFound();
                }
                return Ok(check);
 
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        //[Authorize]
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            try
            {
                var cart = await _cartService.GetCartByIdAsync(id);
                return cart == null ? NotFound() : Ok(cart);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

        }

        [HttpPost]
        public async Task<IActionResult> CreateCart(int proid, int userId)
        {
            try
            {
                var newCartId = await _cartService.CreateCartAsync(proid , userId);
                return Ok();
            }
            catch
            {
                return BadRequest();
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateCart(int id, CartResponse cartResponse)
        {
            try
            {
                await _cartService.UpdateCartAsync(id, cartResponse);
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

        }
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCart(int id)
        {
            try
            {
                await _cartService.DeleteCartAsync(id);
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

        }
        [HttpGet("Pluss")]
        public async Task<IActionResult> PlussCartAsync(int id)
        {
            try
            {
                await _cartService.PlussCartAsync(id);
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

        }
        [HttpGet("Sub")]
        public async Task<IActionResult> SubCartAsync(int id)
        {
            try
            {
                await _cartService.SubCartAsync(id);
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
                CartTotalPrice co = await _cartService.TotalPrice(id);
                return Ok(co);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);

            }
        }
        [HttpGet("CountCart")]
        public async Task<IActionResult> CountCartQuan(int id)
        {
            try
            {
                CountCartQuan co = await _cartService.CountCart(id);
                return Ok(co);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);

            }
        }
        List<CartResponse> carts { get; set; }
        [HttpGet("search")]
        public async Task<IActionResult> SearchProductAsync(string search)
        {

            try
            {
                carts = await _cartService.SearchCartAsync(search);
                return Ok(carts);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
