using Responsitory.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Responsitory.IService
{
    public interface IUserService
    {
        public Task<int> CreateUserAsync(UserResponse UserResponse);
        public Task UpdateUserAsync(int id, UserResponse UserResponse);
        public Task DeleteUserAsync(int id);
        public Task<List<UserResponse>> SearchUserAsync(string search);
        public Task<UserResponse> GetByIdAsync(int id);
        public Task<List<UserResponse>> GetUserAsync();
    }
}
