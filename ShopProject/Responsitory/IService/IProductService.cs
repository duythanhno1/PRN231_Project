using Responsitory.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Responsitory.IService
{
    public interface IProductService
    {
        public Task<int> CreateProductAsync(ProductResponse productResponse);
        public Task UpdateProductAsync(int id, ProductEdit categoryResponse);
        public Task DeleteProductAsync(int id);

        public Task<List<ProductView>> SearchProductAsync(string search);
        public Task<List<ProductView>> SearchProductByCateAsync(int id);
        public Task<ProductView> GetByIdAsync(int id);
        public Task<List<ProductView>> GetProductAsync();
        public Task<CountProduct> CountPro();
    }
}
