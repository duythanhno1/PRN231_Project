using Responsitory.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Responsitory.IService
{
    public interface IOrderDetailService
    {
        public Task<int> CreateOrderDetailAsync(OrderDetailResponse orderdetailResponse);
        public Task UpdateOrderDetailAsync(int id, OrderDetailResponse orderdetailResponse);
        public Task DeleteOrderDetailAsync(int id);
        public Task<List<OrderDetailAdminResponse>> GetOrderDetailAdminAsync( int id);
        public Task<OrderDetailResponse> GetByIdAsync(int id);
        public Task<OrderDetailTotalPriceResponse> TotalPrice(int id);

    }
}
