using BussinessObject.Models;
using Responsitory.DTO;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Responsitory.IService
{
    public interface ICartService
    {
       public Task<int> CreateCartAsync(int proid , int userId);
        public Task DeleteCartAsync(int id);
        public Task<CartResponse> GetCartByIdAsync(int id);
        public Task<List<CartResponse>> SearchCartAsync(string search);
        public Task<List<CustomCartResponse>> GetCartsAsync(int id);
        public List<Cart> CheckCart(List<Cart> carts);
        public Task UpdateCartAsync(int id, CartResponse cartResponse);
        public Task PlussCartAsync(int id);
        public Task SubCartAsync(int id);
        public Task<CountCartQuan> CountCart(int id);
        public Task<CartTotalPrice> TotalPrice(int id);
    }
}
