using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using System;
using Responsitory.DTO;
using Responsitory.IService;
using System.Collections.Generic;
using Microsoft.AspNetCore.Authorization;
using ShopProjectAPI.Helper;

namespace ShopProjectAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class UserController : ControllerBase
    {
        private readonly IUserService _UserService;
        public UserController(IUserService UserService)
        {
            _UserService = UserService;
        }
        [Authorize(Policy = IdentifyRole.AdminRole)]
        [HttpGet]
        public async Task<IActionResult> GetUser()
        {
            try
            {
                return Ok(await _UserService.GetUserAsync());
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
                var User = await _UserService.GetByIdAsync(id);
                return User == null ? NotFound() : Ok(User);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

        }

        [HttpPost]
        public async Task<IActionResult> CreateUser(UserResponse UserResponse)
        {
            try
            {
                var newUserId = await _UserService.CreateUserAsync(UserResponse);
                var User = await _UserService.GetByIdAsync(newUserId);
                return User == null ? NotFound() : Ok(User);
            }
            catch
            {
                return BadRequest();
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateUser(int id, UserResponse UserResponse)
        {
            try
            {
                await _UserService.UpdateUserAsync(id, UserResponse);
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

        }
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUser(int id)
        {
            try
            {
                await _UserService.DeleteUserAsync(id);
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

        }
        List<UserResponse> users{ get; set; }
        [HttpGet("search")]
        public async Task<IActionResult> SearchProductAsync(string search)
        {

            try
            {
                users = await _UserService.SearchUserAsync(search);
                return Ok(users);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
