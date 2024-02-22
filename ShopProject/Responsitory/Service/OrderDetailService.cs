using AutoMapper;
using BussinessObject.Models;
using MailKit.Search;
using Microsoft.EntityFrameworkCore;
using Responsitory.DTO;
using Responsitory.IService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Responsitory.Service
{
    public class OrderDetailService : IOrderDetailService
    {
        private readonly DataAccess.DB.Db _dbContext;
        private readonly IMapper _mapper;
        public User user { get; set; } = new User();
        public Product product { get; set; } = new Product();
       
        public Order order1 { get; set; } = new Order();
        public OrderDetail orderdetail { get; set; }
        public List<OrderDetailAdminResponse> orderdetailadminresponses { get; set; } = new List<OrderDetailAdminResponse>();
        public List<OrderDetail> orderdetails { get; set; }
        public OrderDetailService(DataAccess.DB.Db dbContext, IMapper mapper)
        {
            _dbContext = dbContext;
            _mapper = mapper;
        }
        public async Task<int> CreateOrderDetailAsync(OrderDetailResponse orderdetailResponse)
        {
            orderdetail = new OrderDetail()
            {
                OrderDetailID = orderdetailResponse.OrderDetailID,
                OrderDetailTotalPrice = orderdetailResponse.OrderDetailTotalPrice,
                ProductID = orderdetailResponse.ProductID,
                OrderID = orderdetailResponse.OrderID,
                UserID = orderdetailResponse.UserID,
                OrderDetailStatus = "1"
            };

            _dbContext.OrderDetails.Add(orderdetail);
            await _dbContext.SaveChangesAsync();
            return orderdetail.OrderDetailID;
        }

        public async Task DeleteOrderDetailAsync(int id)
        {
            OrderDetail detemp = _dbContext.OrderDetails.SingleOrDefault(c => c.OrderDetailID == id);
            detemp.OrderDetailStatus = "0";
            _dbContext.OrderDetails.Update(detemp);
            await _dbContext.SaveChangesAsync();
        }

        public async Task<OrderDetailResponse> GetByIdAsync(int id)
        {
            var orderdetail = await _dbContext.OrderDetails.FindAsync(id);
            return _mapper.Map<OrderDetailResponse>(orderdetail);
        }

        public async Task<List<OrderDetailAdminResponse>> GetOrderDetailAdminAsync(int id)
        {
            orderdetails = await _dbContext.OrderDetails.Where(c => c.OrderDetailStatus == "1" && c.OrderID == id).ToListAsync();
            foreach (var order in orderdetails)
            {
                user = _dbContext.Users.Where(u => u.UserID == order.UserID).SingleOrDefault();
                order1 = _dbContext.Orders.Where(v => v.OrderID == order.OrderID).SingleOrDefault();
                product = _dbContext.Products.Where(c => c.ProductID == order.ProductID).SingleOrDefault();
                OrderDetailAdminResponse orderdetailAdminResponse = new OrderDetailAdminResponse()
                {
                    OrderID = order1.OrderID,
                    FullName = user.FullName,
                    Phone = user.Phone,
                    Address = user.Address,
                    OrderDate = order1.OrderDate,
                    OrderQuantity = order1.OrderQuantity,
                    OrderNote = order1.OrderNote,
                    ShipperDate = order1.ShipperDate,
                    ProductName = product.ProductName,
                    ProductImage = product.ProductImage,
                    ProductPrice = product.ProductPrice,
                    ProductQuantity = order.OrderDetailQuantity,
                    OrderDetailTotalPrice = order.OrderDetailTotalPrice

                };
                orderdetailadminresponses.Add(orderdetailAdminResponse);
            }

            return orderdetailadminresponses;
        }

        public async Task UpdateOrderDetailAsync(int id, OrderDetailResponse orderdetailResponse)
        {
            orderdetail = new OrderDetail()
            {
                OrderDetailID = orderdetailResponse.OrderDetailID,
                OrderDetailTotalPrice = orderdetailResponse.OrderDetailTotalPrice,
                //ProductID = orderdetailResponse.ProductID,
                //OrderID = orderdetailResponse.OrderID,
                //CustomerID = orderdetailResponse.CustomerID,
                OrderDetailStatus = "1"
            };
            orderdetail.OrderDetailID = id;
            if (id == orderdetail.OrderDetailID)
            {
                _dbContext.OrderDetails.Update(orderdetail);
                await _dbContext.SaveChangesAsync();
            }
        }
        public async Task<OrderDetailTotalPriceResponse> TotalPrice(int id)
        {
            double total = 0;
            OrderDetailTotalPriceResponse to = new OrderDetailTotalPriceResponse();
            orderdetails = await _dbContext.OrderDetails.Where(C => C.OrderDetailStatus == "1" && C.OrderID == id).ToListAsync();
            foreach (var item in orderdetails)
            {
                to.TotalPrice = to.TotalPrice + item.OrderDetailTotalPrice;
            }
            return to;
        }
    }
}
