using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Responsitory.IService;
using Responsitory.DTO;
using System.Collections.Generic;
using BussinessObject.Models;
using Microsoft.AspNetCore.Authorization;
using ShopProjectAPI.Helper;

namespace ShopProjectAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly IProductService _productService;

        public ProductsController(IProductService productService)
        {
            _productService = productService;
        }
        //[Authorize(Policy = IdentifyRole.AdminRole)]
        [HttpGet]
        public async Task<IActionResult> GetProducts()
        {
            try
            {
                var value = await _productService.GetProductAsync();
                return Ok(value);
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
                var product = await _productService.GetByIdAsync(id);
                return product == null ? NotFound() : Ok(product);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost]
        public async Task<IActionResult> CreateProduct(ProductResponse productResponse)
        {
            try
            {
                var newProductId = await _productService.CreateProductAsync(productResponse);
                var product = await _productService.GetByIdAsync(newProductId);
                return product == null ? NotFound() : Ok(product);
            }
            catch
            {
                return BadRequest();
            }
        }

        [HttpPut]
        public async Task<IActionResult> UpdateProduct(ProductEdit productResponse)
        {
            try
            {
                await _productService.UpdateProductAsync(productResponse.ProductId, productResponse);
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProduct(int id)
        {
            try
            {
                await _productService.DeleteProductAsync(id);
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        List<ProductView> products { get; set; }
        [HttpGet("search")]
        public async Task<IActionResult> SearchProductAsync(string search)
        {
            
            try
            {
                products = await _productService.SearchProductAsync(search);
                return Ok(products);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpGet("searchCate")]
        public async Task<IActionResult> SearchProductByCateAsync(int id)
        {
            
            try
            {
                products = await _productService.SearchProductByCateAsync(id);
                return Ok(products);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpGet("CountProduct")]

        public async Task<IActionResult> Count()
        {
            try
            {
                CountProduct co = await _productService.CountPro();
                return Ok(co);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);

            }

        }
 
    }
}
