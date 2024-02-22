using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using BussinessObject.Models;
using DataAccess.DB;
using Responsitory.IService;
using Responsitory.DTO;
using Microsoft.AspNetCore.Authorization;
using ShopProjectAPI.Helper;

namespace ShopProjectAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Policy = IdentifyRole.AdminRole)]
    public class CategoriesController : ControllerBase
    {
        private readonly ICategoryService _categoryService;
        public CategoriesController(ICategoryService categoryService)
        {
            _categoryService = categoryService;
        }
        [AllowAnonymous]
        [HttpGet]
        public async Task<IActionResult> GetCategory()
        {
            try
            {

                var cate = await _categoryService.GetCategoryAsync();
                if (cate == null)
                {
                    return NotFound();
                }
                return Ok(cate);
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
                var category = await _categoryService.GetByIdAsync(id);
                return category == null ? NotFound() : Ok(category);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

        }
        //[Authorize]
        [HttpPost]
        public async Task<IActionResult> CreateCategory(CategoryResponse categoryResponse)
        {
            try
            {
                var newCategoryId = await _categoryService.CreateCategoryAsync(categoryResponse);
                var category = await _categoryService.GetByIdAsync(newCategoryId);
                return category == null ? NotFound() : Ok(category);
            }
            catch
            {
                return BadRequest();
            }
        }

        [HttpPut]
        public async Task<IActionResult> UpdateCategory(CategoryResponse categoryResponse)
        {
            try
            {
                await _categoryService.UpdateCategoryAsync(categoryResponse.CategoryId, categoryResponse);
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

        }
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCategory(int id)
        {
            try
            {
                await _categoryService.DeleteCategoryAsync(id);
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

        }
        List<CategoryResponse> categorys { get; set; }
        [HttpGet("search")]
        public async Task<IActionResult> SearchProductAsync(string search)
        {

            try
            {
                categorys = await _categoryService.SearchCategoryAsync(search);
                return Ok(categorys);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
